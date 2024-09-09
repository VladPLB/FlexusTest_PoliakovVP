using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gun
{
    public class GunShootTween : MonoBehaviour
    {
        [Serializable]
        public class TransformState
        {
            [SerializeField] private Transform _item;
            [SerializeField] private Vector3 _from;
            [SerializeField] private Vector3 _to;

            public void SetValue(float val)
            {
                if(!_item)
                    return;
                _item.localPosition = Vector3.Lerp(_from, _to, val);
            }
            #if UNITY_EDITOR
            public void SetFrom()
            {
                if(!_item)
                    return;
                _from = _item.localPosition;
            }
            
            public void SetTo()
            {
                if(!_item)
                    return;
                _to = _item.localPosition;
            }
            #endif
        }
        [SerializeField]
        private float _duration = 0.2f;
        [SerializeField]
        private AnimationCurve _ease;

        [SerializeField] private List<TransformState> _items;

        private float effectTime = 0f;
        void Start()
        {
            var gun = SystemsProvider.Get<GunController>();
            gun.OnShoot += Play;
            ResetState();
        }

        public void Play()
        {
            effectTime = _duration;
        }

        private void Update()
        {
            if(effectTime>0)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    _items[i].SetValue( _ease.Evaluate(1f - effectTime / _duration));
                }
                effectTime -= Time.deltaTime;
            }
        }

        [ContextMenu("ResetState")]
        private void ResetState()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].SetValue( 0);
            }
        }
        
        #if UNITY_EDITOR
        [ContextMenu("SetFrom")]
        private void SetFrom()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].SetFrom();
            }
        }
        
        [ContextMenu("SetTo")]
        private void SetTo()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].SetTo();
            }
        }
        #endif
    }
}