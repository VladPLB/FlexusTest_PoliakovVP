
using UnityEngine;

namespace Physic
{
    public struct PhysicsItemState
    {
        public Vector3 Velocity;
        public Vector3 Position;
        public RaycastHit? Hit;

        public PhysicsItemState(Vector3 velocity, Vector3 position, RaycastHit? hit) =>
            (Velocity, Position, Hit) = (velocity, position, hit);
    }
}