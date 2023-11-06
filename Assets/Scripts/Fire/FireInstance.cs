using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Fire
{
    public class FireInstance : MonoBehaviour
    {
        [SerializeField] private float _sparkTimerSpeed = 0.05f;
        [SerializeField] private float _hurtSpeed = 0.5f;
        [SerializeField] private float _fightingSpeed = 1;
        private Vector3 _defaultScale;
        private CapsuleHurtBox _hurtBox;
        private FireSpark _spark;
        private float _sparkTime;
        private float _randomMoment;
        private float _health = 1;
        private readonly List<Burning> _burning = new();

        private void Awake()
        {
            _defaultScale = transform.localScale;
            _hurtBox = GetComponentInChildren<CapsuleHurtBox>();
            _spark = GetComponentInChildren<FireSpark>(includeInactive: true);
        }

        private void OnEnable()
        {
            transform.localScale = _defaultScale;
            _hurtBox.OnEnter += OnEnter;
            _hurtBox.OnStay += OnStay;
            _hurtBox.OnExit += OnExit;
            _health = 1;
            _sparkTime = 1;
            _randomMoment = Random.value;
        }

        private void OnEnter(Collider other)
        {
            if (other.TryGetComponent(out HealthSystem health))
            {
                _burning.Add(new(health));
            }
        }

        private void OnStay(Collider other)
        {
            for (int i = 0; i < _burning.Count; i++)
            {
                if (_burning[i].Time >= 1)
                {
                    _burning[i].Health.Damage();
                    _burning[i].Time = 0;
                    continue;
                }

                _burning[i].Time += _hurtSpeed * Time.deltaTime;
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if (_health > 0.3f)
            {
                _health -= _fightingSpeed * Time.deltaTime;
                transform.localScale = _defaultScale * _health;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
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
                _burning
                    .Remove(_burning
                    .SingleOrDefault(burning => burning.Health == health));
            }
        }

        private void OnDisable()
        {
            _hurtBox.OnEnter -= OnEnter;
            _hurtBox.OnStay -= OnStay;
            _hurtBox.OnExit -= OnExit;
        }
    }
}