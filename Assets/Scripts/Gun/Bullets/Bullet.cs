using System;
using _GAME.Scripts.Pools;
using Core;
using Physic;
using Tools;
using UnityEngine;

namespace Gun.Bullets
{
    public class Bullet : MonoBehaviour, IPoolableItem<BulletType>, IPhysicsItem
    {
        private PhysicsSystem _physicsSystem;
        private BulletsProvider _bulletsProvider;
        
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private BulletType _type;
        [SerializeField] private float _mass = 1f;
        [SerializeField] private float _drag = .1f;
        [SerializeField] private int _maxBounce = 2;
        [SerializeField] private Texture2D _hitTexture;

        private int _bounceCount = 0;
        
        public BulletType Type => _type;

        public GameObject GameObject => gameObject;
        public float Mass => _mass;
        public float Drag => _drag;
        public int MaxBounce => _maxBounce;

        public Vector3 Size { get; private set; } = Vector3.one;
        public Vector3 Velocity { get; set; } = Vector3.zero;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private void Awake()
        {
            _physicsSystem = SystemsProvider.Get<PhysicsSystem>();
            _bulletsProvider = SystemsProvider.Get<BulletsProvider>();
        }

        public void Setup(Vector3 size, Mesh mesh)
        {
            Size = size;
            _meshFilter.mesh = mesh;
            _bounceCount = 0;
            _physicsSystem.TryRegistryItem(this);
        }
        
        public void OnBounce(RaycastHit hit)
        {
            _bounceCount++;
            if(_bounceCount>= MaxBounce)
            {
                var hitPainter = hit.transform.GetComponent<TexturePainter>();
                if (hitPainter)
                {
                    hitPainter.Paint(transform.position,hit.normal*-1f, _hitTexture);
                }
                Remove();
            }
        }

        private void Remove()
        {
            _physicsSystem.RemoveItem(this);
            _bulletsProvider.RemoveBullet(this);
        }
    }
}