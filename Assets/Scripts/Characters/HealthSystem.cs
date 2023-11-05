using Assets.Scripts.Gameplay_UI;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    private HealthBar _healthBar;

    private ICharacter _character;

    private void Start()
    {
        _character = transform.GetComponent<ICharacter>();
        _healthBar = FindAnyObjectByType<HealthBar>();
        _healthBar.Set(_health);
    }

    public void Damage(int value = 1)
    {
        _health -= value;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        _healthBar.Set(_health);

        if (_health <= 0)
        {
            if (_character.IsDead == false)
            {
                _character.Dead();
            }
        }
    }

    public void Heal(int value = 1)
    {
        _health += value;
        _healthBar.Set(_health);

        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public float GetMaxHealth() => _maxHealth;

    public float GetHealth() => _health;
}
