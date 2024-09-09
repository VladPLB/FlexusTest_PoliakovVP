using UnityEngine;

namespace Physic
{
    public interface IPhysicsItem
    {
        GameObject GameObject { get; }
        Vector3 Size {get;}
        float Mass { get; }
        float Drag { get; }
        int MaxBounce { get; }
        Vector3 Velocity { get; set; }
        Vector3 Position { get; set; }

        public void OnBounce(RaycastHit hit);
    }
}