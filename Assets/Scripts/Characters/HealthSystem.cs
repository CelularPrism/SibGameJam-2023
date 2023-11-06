using Assets.Scripts.Gameplay_UI;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxAditionalHealth;
    [SerializeField] private EventReference _dmgEvent;
    private int _aditionalHealth;
    private HealthBar _healthBar;

    private ICharacter _character;
    private Vignette _vignette;
    private float _defaultVignetteIntens;
    private float _vignetteShift;

    private void Awake()
    {
        _aditionalHealth = _maxAditionalHealth;
        _vignette = FindAnyObjectByType<PostProcessVolume>().profile.GetSetting<Vignette>();
        _defaultVignetteIntens = _vignette.intensity;
        _vignetteShift = _defaultVignetteIntens / _maxHealth;
    }

    private void Start()
    {
        _character = transform.GetComponent<ICharacter>();
        _healthBar = FindAnyObjectByType<HealthBar>();
        _healthBar.Set(_health);
    }

    public void Damage(int value = 1)
    {
        _aditionalHealth -= value;
        _aditionalHealth = Mathf.Clamp(_aditionalHealth, 0, _maxAditionalHealth);
        RuntimeManager.PlayOneShot(_dmgEvent, transform.position);

        if (_aditionalHealth <= 0)
        {
            _vignette.intensity.value += _vignetteShift;
            _aditionalHealth = _maxAditionalHealth;
            _health--;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            _healthBar.Set(_health);

            if (_health <= 0)
            {
                if (_character.IsDead == false)
                {
                    _vignette.intensity.value = _defaultVignetteIntens;
                    _character.Dead();
                }
            }
        }
    }

    public void Heal(int value = 1)
    {
        _vignette.intensity.value -= _vignetteShift;
        _health += value;
        _healthBar.Set(_health);

        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public float GetMaxHealth() => _maxHealth;

    public float GetHealth() => _health;

    public bool IsMax() => _health == _maxHealth;
}
