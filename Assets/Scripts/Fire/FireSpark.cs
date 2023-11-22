using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Assets.Scripts.Fire
{
    public class FireSpark : MonoBehaviour
    {
        [SerializeField] private float _checkRadius = 1;
        private ParticleSystem _particleSystem;
        private Particle[] _particles;
        private RaycastHit _hitInfo;

        private void LateUpdate()
        {
            if (_particleSystem == null)
                _particleSystem = GetComponent<ParticleSystem>();

            if (_particles == null || _particles.Length < _particleSystem.main.maxParticles)
                _particles = new Particle[_particleSystem.main.maxParticles];

            _particleSystem.GetParticles(_particles);
        }

        //private void OnParticleCollision(GameObject other) => Fire();

        private void OnDisable() => Fire();

        private void Fire()
        {
            Vector3 position = transform.TransformPoint(_particles[0].position);
            position.y = Mathf.Abs(position.y) + 10;

            if (Physics.SphereCast(position, _checkRadius, Vector3.down, out _hitInfo, maxDistance: 100, 
                LayerMask.GetMask("Ground", "Fire", "Usable Item", "NoFire")))
            {
                if ((LayerMask.GetMask("Fire") & (1 << _hitInfo.transform.gameObject.layer)) > 0)
                    return;

                if ((LayerMask.GetMask("Usable Item") & (1 << _hitInfo.transform.gameObject.layer)) > 0)
                    return;

                if ((LayerMask.GetMask("NoFire") & (1 << _hitInfo.transform.gameObject.layer)) > 0)
                    return;

                FireInstance fire = GetComponentInParent<FireInstance>();

                if (fire)
                    Instantiate(fire.gameObject, _hitInfo.point, Quaternion.identity);
            }
        }

        public void Create() => gameObject.SetActive(true);
    }
}