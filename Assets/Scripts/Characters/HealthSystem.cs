using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;

    private ICharacter _character;

    private void Start()
    {
        _character = transform.GetComponent<ICharacter>();
    }

    public void Damage(float value = 1)
    {
        _health -= value;

        if (_health <= 0)
        {
            if (_character.IsDead == false)
            {
                _health = 0;
                _character.Dead();
            }
        }
    }

    public void Heal(float value = 1)
    {
        _health += value;
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public float GetMaxHealth() => _maxHealth;

    public float GetHealth() => _health;
}
