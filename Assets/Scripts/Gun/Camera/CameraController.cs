using UnityEngine;

namespace Gun
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CameraSettings _settings;
        [SerializeField] private Transform _transform;
        
        private Vector2 _rotation = Vector2.zero;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * _settings.MouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _settings.MouseSensitivity * Time.deltaTime;
            
            _rotation.x = Mathf.Clamp(_rotation.x - mouseY, -_settings.RotationConstraints.y, _settings.RotationConstraints.y);
            _rotation.y = Mathf.Clamp(_rotation.y + mouseX, -_settings.RotationConstraints.x, _settings.RotationConstraints.x);

            _transform.localRotation = Quaternion.Euler(_rotation);
        }
    }
}
