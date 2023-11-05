using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Fire
{
    public class FireInstance : MonoBehaviour
    {
        [SerializeField] private float _sparkTimerSpeed = 0.05f;
        [SerializeField] private float _hurtSpeed = 0.5f;
        private FireSpark _spark;
        private float _sparkTime;
        private float _randomMoment;
        private readonly List<Burning> _burning = new();

        private void Awake() => _spark = GetComponentInChildren<FireSpark>(includeInactive: true);

        private void OnEnable()
        {
            _spark.gameObject.SetActive(false);
            _sparkTime = 0;
            _randomMoment = Random.value;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HealthSystem health))
            {
                _burning.Add(new(health));
            }
        }

        private void OnTriggerStay(Collider other)
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

        private void Update()
        {
            if (_sparkTime >= 1)
            {
                CreateSpark();
                return;
            }

            if (Mathf.Approximately(_sparkTime, _randomMoment))
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

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out HealthSystem health))
            {
                _burning
                    .Remove(_burning
                    .SingleOrDefault(burning => burning.Health == health));
            }
        }
    }
}