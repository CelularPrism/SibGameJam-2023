using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Assets.Scripts.Characters
{
    public class VignetteController : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _limit;
        private Vignette _vignette;
        private HealthSystem _health;
        private float _defaultVignetteIntens;

        private void Awake()
        {
            _vignette = FindAnyObjectByType<PostProcessVolume>().profile.GetSetting<Vignette>();
            _health = FindAnyObjectByType<HealthSystem>();
            _defaultVignetteIntens = _vignette.intensity.value;

        }

        private void OnEnable() => _health.OnChange += OnHealthChanged;

        private void OnHealthChanged(float value)
        {
            value = (_health.MaxHealth - value) / _health.MaxHealth;
            value = Mathf.Clamp(value, _defaultVignetteIntens, _limit);
            _vignette.intensity.value = value;
        }

        private void OnDisable() => _health.OnChange -= OnHealthChanged;
    }
}