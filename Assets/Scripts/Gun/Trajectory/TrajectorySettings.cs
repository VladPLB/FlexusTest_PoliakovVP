using UnityEngine;

namespace Gun
{
    [CreateAssetMenu(fileName = "TrajectorySettings", menuName = "Scriptable/Trajectory/Settings", order = 0)]
    public class TrajectorySettings : ScriptableObject
    {
        [SerializeField, Min(0)] private int _maxLenght = 100;
        [SerializeField, Min(0)] private int _maxBounce = 0;

        public int MaxLenght => _maxLenght;
        public int MaxBounce => _maxBounce;
    }
}