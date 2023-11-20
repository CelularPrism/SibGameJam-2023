using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.Scripts.Characters
{
    public class VignetteController : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _limit;
        [SerializeField] private AnimationCurve _pulsationCurve;
        [SerializeField] private float _maxPulsationFrequency;
        private Vignette _vignette;
        private HealthSystem _health;
        private float _defaultVignetteIntens;
        private float _time;

        private void Awake()
        {
            FindAnyObjectByType<Volume>().profile.TryGet(out _vignette);
            _health = FindAnyObjectByType<HealthSystem>();
            _defaultVignetteIntens = _vignette.intensity.value;

        }

        private void OnEnable() => _health.OnChange += OnHealthChanged;

        private void Update()
        {
            float value = (_health.MaxHealth - _health.Health) / _health.MaxHealth;
            float clampedValue = Mathf.Clamp(value, _defaultVignetteIntens, _limit);

            if (_maxPulsationFrequency > 0)
            {
                if (_time > 1)
                    _time = 0;

                clampedValue += _pulsationCurve.Evaluate(_time);
                _time += _maxPulsationFrequency * value * Time.deltaTime;
            }

            _vignette.intensity.value = clampedValue;
        }

        private void OnHealthChanged(float value)
        {
            
        }

        private void OnDisable() => _health.OnChange -= OnHealthChanged;
    }
}