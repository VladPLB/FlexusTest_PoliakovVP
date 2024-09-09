using System;
using UnityEngine;

namespace Gun
{
    [CreateAssetMenu(fileName = "GunSettings", menuName = "Scriptable/Gun/Settings", order = 0)]
    public class GunSettings : ScriptableObject
    {
        [SerializeField, Min(0)] private float _minForce = 5;
        [SerializeField, Min(0)] private float _maxForce = 30;
        [SerializeField, Min(0)] private float _scrollSpeed = 10;

        public float MinForce => _minForce;
        public float MaxForce => _maxForce;
        
        public float ScrollSpeed => _scrollSpeed;

        private void OnValidate()
        {
            _maxForce = Mathf.Max(_maxForce, _minForce);
        }
    }
}