using System.Collections.Generic;
using System.Linq;
using Gun.Bullets;
using UnityEngine;

namespace Gun
{
    [CreateAssetMenu(fileName = "BulletsDatabase", menuName = "Scriptable/Bullets/Database", order = 0)]
    public class BulletsDatabase : ScriptableObject
    {
        [SerializeField] private List<Bullet> _prefabs;
        private Dictionary<BulletType, Bullet> _prefabByType = null;

        public Bullet GetBulletPrefab(BulletType type)
        {
            if (_prefabByType == null)
            {
                _prefabByType = _prefabs.ToDictionary(p => p.Type);
            }

            if (_prefabByType.ContainsKey(type))
                return _prefabByType[type];

            return null;
        }
    }
}