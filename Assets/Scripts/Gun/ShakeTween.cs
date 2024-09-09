using Core;
using UnityEngine;

namespace Gun
{
    public class ShakeTween : MonoBehaviour
    {
        [SerializeField]
        private float _shakeDuration = 0.2f;
        [SerializeField]
        private float _shakeMagnitude = 0.2f;

        private float effectTime = 0f;
        private Vector3 _defaultPosition;

        void Start()
        {
            _defaultPosition = transform.localPosition;
            var gun = SystemsProvider.Get<GunController>();
            gun.OnShoot += Shake;
        }

        public void Shake()
        {
            effectTime = _shakeDuration;
        }

        private void Update()
        {
            if(effectTime>0)
            {
                var magnitude = Mathf.Lerp(_shakeMagnitude, 0, 1f - effectTime / _shakeDuration);
                Vector3 randomPoint = _defaultPosition + Random.insideUnitSphere * magnitude;
                transform.localPosition = randomPoint;
                effectTime -= Time.deltaTime;
            }
            else
            {
                transform.localPosition = _defaultPosition;
            }
        }
    }
}