using UnityEngine;

namespace Gun
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Scriptable/Camera/Settings", order = 0)]
    public class CameraSettings : ScriptableObject
    {
        [SerializeField, Min(0)] private float _mouseSensitivity = 300;
        [SerializeField] private Vector2 _rotationConstraints = Vector2.one * 60;

        public float MouseSensitivity => _mouseSensitivity;
        public Vector2 RotationConstraints => _rotationConstraints;
    }
}