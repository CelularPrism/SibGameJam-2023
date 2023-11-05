﻿using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Assets.Scripts.Fire
{
    public class FireSpark : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
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

        private void OnParticleCollision(GameObject other) => Fire();

        private void OnDisable() => Fire();

        private void Fire()
        {
            Vector3 position = transform.TransformPoint(_particles[0].position);
            position.y = Mathf.Abs(position.y);

            if (Physics.SphereCast(position, radius: 0.2f, Vector3.down, out _hitInfo))
            {
                //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = _hitInfo.point;
                Instantiate(transform.root, _hitInfo.point, Quaternion.identity);
            }
        }

        public void Create() => gameObject.SetActive(true);
    }
}