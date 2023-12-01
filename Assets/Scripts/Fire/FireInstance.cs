using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Fire
{
    public class FireInstance : MonoBehaviour
    {
        [SerializeField] private float _sparkTimerSpeed = 0.05f;
        [SerializeField] private float _hurtSpeed = 0.5f;
        [SerializeField] private float _fightingSpeed = 1;
        [SerializeField] private float _firstDamage;
        [SerializeField] private float _damage;
        [SerializeField] private float _scale;
        [SerializeField] private bool _canSpread, _canRestore;
        [SerializeField] private float _restoreDelay;
        [SerializeField] private float _deathSize = 0.6f;
        private CapsuleHurtBox _hurtBox;
        private FireSpark _spark;
        private ParticleSystem _particleSystem;
        private float _sparkTime;
        private float _randomMoment;
        private float _health = 1;
        private float _restoreTime;
        private FireMediator _mediator;
        private readonly List<Burning> _burning = new();
        private readonly List<HealthSystem> _damagables = new();

        private void Awake()
        {
            _hurtBox = GetComponentInChildren<CapsuleHurtBox>();
            _spark = GetComponentInChildren<FireSpark>(includeInactive: true);
            _particleSystem = GetComponent<ParticleSystem>();
            _mediator = FindObjectOfType<FireMediator>();

            if (_mediator == null)
                _mediator = new GameObject("[FIRE MEDIATOR]").AddComponent<FireMediator>();

            _mediator.Add(this);
        }

        private void OnEnable()
        {
            _hurtBox.OnEnter += OnEnter;
            _hurtBox.OnExit += OnExit;
            _health = 1;
            _sparkTime = 0;
            _randomMoment = Random.value;
            transform.localScale = Vector3.one * _scale;
            _particleSystem.Clear();
        }

        private void OnEnter(Collider other)
        {
            if (other.TryGetComponent(out HealthSystem health))
            {
                health.Damage(_firstDamage);
                _damagables.Add(health);
                health.AddDamage<FireInstance>(_damage);
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if (_health > _deathSize)
            {
                _health -= _fightingSpeed * Time.deltaTime;
                transform.localScale = (Vector3.one * _scale) * _health;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (_canSpread)
            {
                if (_mediator.IsLimitReached)
                    return;

                if (_sparkTime >= 1)
                {
                    CreateSpark();
                    return;
                }

                if (_sparkTime >= _randomMoment)
                {
                    CreateSpark();
                    return;
                }

                _sparkTime += _sparkTimerSpeed * Time.deltaTime;
            }
        }

        private void CreateSpark()
        {
            _spark.Create();
            _randomMoment = Random.value;
            _sparkTime = 0;
            return;
        }

        private void OnExit(Collider other)
        {
            if (other.TryGetComponent(out HealthSystem health))
            {
                health.RemoveDamage<FireInstance>();
            }
        }

        private void OnDisable()
        {
            _hurtBox.OnEnter -= OnEnter;
            _hurtBox.OnExit -= OnExit;

            if (_canRestore)
                Invoke(nameof(Restore), _restoreDelay);

            _damagables.ForEach(damagable => damagable.RemoveDamage<FireInstance>());
        }

        private void Restore() => gameObject.SetActive(true);

        public void SetSpreed(bool can) => _canSpread = can;

        public void SetRestore(bool can) => _canRestore = can;
    }
}