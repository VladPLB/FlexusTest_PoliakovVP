using System;
using _GAME.Scripts;
using Core;
using Tools;
using UnityEngine;

namespace Gun.Bullets
{
    public class BulletsProvider : MonoBehaviour, IRuntimeSetup
    {
        [SerializeField] private BulletsDatabase _database;
        private Pool<Bullet, BulletType> _bulletsPool;
        private RandomizedBoxMeshFactory _meshFactory;

        private void Awake()
        {
            SystemsProvider.Registry(this);
        }

        public void RuntimeSetup()
        {
            _bulletsPool = new Pool<Bullet, BulletType>(_database.GetBulletPrefab);
            _meshFactory = new RandomizedBoxMeshFactory();
        }

        public Bullet GetBullet(BulletType type, float size)
        {
            var bullet = _bulletsPool.Pop(type);
            var mesh = _meshFactory.CreateMesh(size);
            bullet.Setup(Vector3.one * size, mesh);
            return bullet;
        }

        public void RemoveBullet(Bullet bullet)
        {
            _bulletsPool.Push(bullet);
        }
    }
}