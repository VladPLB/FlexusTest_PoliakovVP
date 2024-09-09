using UnityEngine;

namespace Physic
{
    [CreateAssetMenu(fileName = "PhysicsSettings", menuName = "Scriptable/Physics/Settings", order = 0)]
    public class PhysicsSettings : ScriptableObject
    {
        [SerializeField] private Vector3 _gravityForce = new Vector3(0, -9.81f,0);
        [SerializeField] private float _airDrag = 0.1f;
        [SerializeField] private float _minAcceleration = 0.001f;
        [SerializeField] private int _boundsCorrectionSteps = 4;
        [SerializeField] private LayerMask _collisionMask;

        public Vector3 GravityForce => _gravityForce;
        public float AirDrag => _airDrag;
        public float MinAcceleration => _minAcceleration;
        public int BoundsCorrectionSteps => _boundsCorrectionSteps;
        public LayerMask CollisionMask => _collisionMask;
    }
}