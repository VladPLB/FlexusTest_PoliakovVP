using System;
using Core;
using Gun;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ForceBarViewer : MonoBehaviour
    {
        [SerializeField] private Image _fillBar;
        [SerializeField] private Text _valueLabel;

        private float _labelMoveMaxHeight = 0f;

        private void Start()
        {
            var gun = SystemsProvider.Get<GunController>();
            gun.OnChangeForce += OnChangeForce;
            gun.OnChangeForceValue += OnChangeForceValue;
            
            _labelMoveMaxHeight = _fillBar.rectTransform.rect.height;

            OnChangeForce(gun.FireForce);
            OnChangeForceValue(gun.FireForceValue);
        }

        private void OnChangeForce(float value)
        {
            _valueLabel.text = $"{value :0.0}";
        }
        private void OnChangeForceValue(float forceValue)
        {
            _fillBar.fillAmount = forceValue;
            _valueLabel.rectTransform.anchoredPosition = new Vector2(_valueLabel.rectTransform.anchoredPosition.x,
                _labelMoveMaxHeight * forceValue);
        }
    }
}