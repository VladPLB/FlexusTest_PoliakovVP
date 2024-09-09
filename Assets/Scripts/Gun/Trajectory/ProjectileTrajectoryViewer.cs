using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Physic;
using UnityEngine;

namespace Gun
{
    public class ProjectileTrajectoryViewer : MonoBehaviour
    {
        [SerializeField] private TrajectorySettings _settings;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Transform _firePoint;

        private Vector3 _trajectoryForward;

        private float _force = 30f;
        private float _newForce = 30f;

        private Vector3 _itemSize = Vector3.one;
        private float _itemMass = 1f;
        private float _itemDrag = 0.01f;

        private PhysicsSystem _physics;

        public void Init(GunController gunController)
        {
            _physics = SystemsProvider.Get<PhysicsSystem>();

            gunController.OnChangeForce += OnChangeForce;
            _force = gunController.FireForce;
            
            _lineRenderer.positionCount = 0;
            _trajectoryForward = _firePoint.forward;
            DrawTrajectory();
        }

        public void Setup(IPhysicsItem itemData)
        {
            _itemSize = itemData.Size;
            _itemMass = itemData.Mass;
            _itemDrag = itemData.Drag;
            DrawTrajectory();
        }

        private void OnChangeForce(float force)
        {
            _newForce = force;
        }

        private void FixedUpdate()
        {
            if (_trajectoryForward != _firePoint.forward || _newForce != _force)
            {
                _force = _newForce;
                _trajectoryForward = _firePoint.forward;
                DrawTrajectory();
            }
        }

        private void DrawTrajectory()
        {
            var trajectoryPoints = GetTrajectoryPoints(out _);
            _lineRenderer.positionCount = trajectoryPoints.Count;
            _lineRenderer.SetPositions(trajectoryPoints.ToArray());
            _lineRenderer.Simplify(.01f);
        }

        private List<Vector3> GetTrajectoryPoints(out List<Vector3> bouncePositions)
        {
            return _physics.TrajectorySimulate(_firePoint.position, _trajectoryForward * _force, _itemSize, _itemMass,
                _itemDrag, _settings.MaxLenght,
                _settings.MaxBounce, out bouncePositions);
        }
    }
}
