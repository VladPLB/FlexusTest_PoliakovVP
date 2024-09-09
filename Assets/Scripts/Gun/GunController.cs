using System;
using Core;
using Gun.Bullets;
using Physic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gun
{
    public class GunController: MonoBehaviour, IRuntimeSetup
    {
        private BulletsProvider _bulletsProvider;
        [SerializeField] private GunSettings _settings;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private ProjectileTrajectoryViewer _projectileTrajectoryViewer;
        
        private float _fireForce = 30f;

        public float FireForce
        {
            get => _fireForce;
            set
            {
                var oldValue = _fireForce;
                _fireForce = Mathf.Clamp(value, _settings.MinForce, _settings.MaxForce);
                if(Mathf.Abs(_fireForce - oldValue)>=Single.Epsilon)
                {
                    OnChangeForce?.Invoke(_fireForce);
                    OnChangeForceValue?.Invoke(FireForceValue);
                }
            }
        }

        public float FireForceValue => _settings.MaxForce <= _settings.MinForce
            ? 1f
            : (_fireForce - _settings.MinForce) / (_settings.MaxForce - _settings.MinForce);
        
        public event Action<float> OnChangeForce;
        public event Action<float> OnChangeForceValue;
        public event Action OnShoot; 

        private void Awake()
        {
            SystemsProvider.Registry(this, typeof(BulletsProvider), typeof(PhysicsSystem));
        }

        public void RuntimeSetup()
        {
            _bulletsProvider = SystemsProvider.Get<BulletsProvider>();
            _projectileTrajectoryViewer.Init(this);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
            
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            FireForce += scrollWheel * _settings.ScrollSpeed;
        }

        private void Shoot()
        {
            var bullet = _bulletsProvider.GetBullet(BulletType.Box, Random.Range(.25f, .35f));
            bullet.Position = _firePoint.position;
            bullet.Velocity = _firePoint.forward * _fireForce;
            OnShoot?.Invoke();
        }
    }
}