using Assets.Scripts.Gameplay_UI;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action<float> OnChange;

    [field: SerializeField] public float MaxHealth { get; private set; }
    [SerializeField] private float _health, _restoreDelay, _restoreSpeed;
    [SerializeField] private Material _damageIndicationMaterial;
    [SerializeField] private Color _damageIndicationColor;
    [SerializeField] private float _hurtIndicationSmooth;
    [SerializeField] private float _hurtSoundInterval;
    [SerializeField] private EventReference _dmgEvent;
    private bool _isDamage;
    private HealthBar _healthBar;
    private ICharacter _character;
    private readonly Dictionary<Type, float> _damegeEffects = new();
    private float _restoreTime, _hurtSoundTime;
    private SkinnedMeshRenderer _meshRenderer;

    public float Health 
    { 
        get => _health;

        private set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            OnChange?.Invoke(_health);

            if (_healthBar)
                _healthBar.Set(_health);

            if (_health <= 0)
            {
                _character.Dead();
            }
        }
    }

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        _hurtSoundTime = _hurtSoundInterval;
        _character = transform.GetComponent<ICharacter>();
        _healthBar = FindAnyObjectByType<HealthBar>();

        if (_healthBar)
            _healthBar.Set(Health);
    }

    private void Update()
    {
        if (_isDamage)
        {
            if (_hurtSoundTime >= _hurtSoundInterval)
            {
                _hurtSoundTime = 0;
                RuntimeManager.PlayOneShot(_dmgEvent, transform.position);
                _damageIndicationMaterial.color = _damageIndicationColor;
                _meshRenderer.material = _damageIndicationMaterial;
            }
            else
            {
                _hurtSoundTime += Time.deltaTime;
            }
        }
        else
        {
            _hurtSoundTime = _hurtSoundInterval;
        }

        _damageIndicationMaterial.color = Color.Lerp(_damageIndicationMaterial.color, Color.white, _hurtIndicationSmooth * Time.deltaTime);

        if (_damegeEffects.Count == 0)
        {
            if (Health == MaxHealth)
            {
                _restoreTime = 0;
                return;
            }

            if (_restoreTime >= _restoreDelay)
            {
                Health += _restoreSpeed * Time.deltaTime;
                return;
            }

            _restoreTime += Time.deltaTime;
        }

        for (int i = 0; i < _damegeEffects.Values.Count; i++)
        {
            Health -= _damegeEffects.Values.ElementAt(i) * Time.deltaTime;
            _restoreTime = 0;
        }
    }

    public void Damage(float value = 1)
    {
        if (_character.IsDead)
            return;

        Health -= value;
        _restoreTime = 0;
        RuntimeManager.PlayOneShot(_dmgEvent, transform.position);
    }

    public void AddDamage<T>(float value)
    {
        if (_damegeEffects.ContainsKey(typeof(T)))
        {
            float damage = _damegeEffects[typeof(T)];

            if (damage < value)
            {
                _damegeEffects[typeof(T)] = value;
                return;
            }

            return;
        }

        _damegeEffects.Add(typeof(T), value);
        _isDamage = true;
    }

    public void RemoveDamage<T>()
    {
        _damegeEffects.Remove(typeof(T));

        if (_damegeEffects.Count == 0)
            _isDamage = false;
    }

    public void Heal(int value = 1) => Health += value;

    public bool IsMax() => Health == MaxHealth;
}