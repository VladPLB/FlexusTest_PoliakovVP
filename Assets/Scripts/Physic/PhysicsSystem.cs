using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Physic
{
    public class PhysicsSystem: MonoBehaviour, IRuntimeSetup
    {
        [SerializeField] private PhysicsSettings _settings;

        private List<IPhysicsItem> _physicItems = new();
        private List<IPhysicsItem> _removedItems = new();
        private void Awake()
        {
            SystemsProvider.Registry(this);
        }
        
        public void RuntimeSetup()
        {
           Physics.simulationMode = SimulationMode.Script;
        }

        public bool TryRegistryItem(IPhysicsItem item)
        {
            if (_physicItems.Contains(item))
                return false;
            
            _physicItems.Add(item);
            return true;
        }

        public void RemoveItem(IPhysicsItem item)
        {
            if (_physicItems.Contains(item) && !_removedItems.Contains(item))
                _removedItems.Add(item);
        }

        private void FixedUpdate()
        {
            for (int i = _physicItems.Count-1; i >=0; i--)
            {
                if (!CheckItem(i))
                {
                    continue;
                }

                SimulateItem(_physicItems[i]);
            }
            
            ClearRemovedItems();
        }

        private bool CheckItem(int i)
        {
            if (!_physicItems[i].GameObject)
            {
                _physicItems.RemoveAt(i);
                return false;
            }

            return _physicItems[i].GameObject.activeInHierarchy;
        }

        private void ClearRemovedItems()
        {
            for (int i = 0; i < _removedItems.Count; i++)
            {
                if(_removedItems[i]==null)
                    continue;
                if(!_physicItems.Contains(_removedItems[i]))
                    continue;
                _physicItems.Remove(_removedItems[i]);
            }
            _removedItems.Clear();
        }

        private void SimulateItem(IPhysicsItem item)
        {
            var nextState = SimulateItem(item.Velocity, item.Position, item.Size, item.Mass, item.Drag);
            item.Velocity = nextState.Velocity;
            item.Position = nextState.Position;
            if (nextState.Hit!=null)
            {
                item.OnBounce(nextState.Hit.Value);
            }
        }

        private PhysicsItemState SimulateItem(Vector3 velocity, Vector3 position, Vector3 size, float mass, float drag)
        {
            var halfExtents = size * .5f;
            velocity = GetUpdatedVelocity(velocity, mass, drag);
            Vector3 previousPosition = position;
            Vector3 nextPosition = GetNextPosition(velocity, previousPosition );
            var moveMagnitude = velocity == Vector3.zero? 0f: (nextPosition - previousPosition).magnitude;
            
            bool isSleep = moveMagnitude <= _settings.MinAcceleration;
            RaycastHit? bounceHit = null;

            if (!isSleep)
            {
                if (CheckItemBounce(velocity, previousPosition, halfExtents, moveMagnitude,
                        out var hit))
                {
                    var offset = velocity.normalized * hit.distance * -1f;
                    nextPosition = previousPosition + velocity.normalized * moveMagnitude;
                    nextPosition -= offset;
                    velocity = Vector3.Reflect(velocity, hit.normal);
                    bounceHit = hit;
                }
            }

            return new PhysicsItemState(velocity, nextPosition, bounceHit);
        }

        private Vector3 GetUpdatedVelocity(Vector3 inVelocity, float mass, float drag)
        {
            var gravityForce = (mass > 0 ? _settings.GravityForce : Vector3.zero) * Time.deltaTime;
            var outVelocity = inVelocity + gravityForce;
            var dragForce = outVelocity * (drag + _settings.AirDrag) * Time.deltaTime;
            outVelocity -= dragForce;
            return outVelocity;
        }
        
        private Vector3 GetNextPosition(Vector3 velocity, Vector3 previousPosition)
        {
            if (velocity == Vector3.zero)
                return previousPosition;
            return previousPosition + velocity * Time.fixedDeltaTime;
        }

        private bool CheckItemBounce(Vector3 velocity, Vector3 previousPosition, Vector3 halfExtents, float moveMagnitude, out RaycastHit hit)
        {
            var direction = velocity.normalized;
            Debug.DrawLine(previousPosition, previousPosition + direction * moveMagnitude, Color.green);
            if (Physics.BoxCast(previousPosition ,halfExtents, direction, out hit, Quaternion.identity, moveMagnitude,
                    _settings.CollisionMask))
            {
                Debug.DrawRay(hit.point, Vector3.up, Color.red);
                return true;
            }

            return false;
        }

        public List<Vector3> TrajectorySimulate(Vector3 startPosition, Vector3 startForce, Vector3 size, float mass, float drag, int steps, int maxBounds, out List<Vector3> bouncePositions)
        {
            List<Vector3> outList = new(){startPosition};
            bouncePositions = new();
            mass = Mathf.Max(0, mass);
            Vector3 velocity = startForce.normalized *  (mass > 0 ? startForce.magnitude / mass : startForce.magnitude);
            var state = new PhysicsItemState(velocity, startPosition, null);
            int bounceCount = 0;
            for (int i = 0; i < steps; i++)
            {
                state = SimulateItem(state.Velocity, state.Position, size, mass, drag);
                outList.Add(state.Position);
                if (state.Hit!=null)
                {
                    bounceCount++;
                    bouncePositions.Add(state.Position);
                    if (bounceCount > maxBounds)
                        break;
                }
            }

            return outList;
        }
    }
}