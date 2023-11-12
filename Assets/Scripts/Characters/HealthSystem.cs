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
    [SerializeField] private float _health;
    [SerializeField] private EventReference _dmgEvent;
    private HealthBar _healthBar;
    private ICharacter _character;
    private Dictionary<Type, float> _damegeEffects = new();
    public float Health 
    { 
        get => _health;

        private set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            OnChange?.Invoke(_health);
            _healthBar.Set(_health);

            if (_health <= 0)
            {
                _character.Dead();
            }
        }
    }

    private void Start()
    {
        _character = transform.GetComponent<ICharacter>();
        _healthBar = FindAnyObjectByType<HealthBar>();
        _healthBar.Set(Health);
    }

    private void Update()
    {
        for (int i = 0; i < _damegeEffects.Values.Count; i++)
        {
            Health -= _damegeEffects.Values.ElementAt(i) * Time.deltaTime;
        }
    }

    public void Damage(float value = 1)
    {
        if (_character.IsDead)
            return;

        Health -= value;
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
    }

    public void RemoveDamage<T>() => _damegeEffects.Remove(typeof(T));

    public void Heal(int value = 1) => Health += value;

    public bool IsMax() => Health == MaxHealth;
}