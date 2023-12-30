using System;
using UnityEngine;

namespace Assets._Project.Gameplay.Ground_Check
{
    public class CapsuleGroundChecker : IGroundChecker
    {
        public event Action OnGrounded;

        private Collider[] _groundColliders = new Collider[1];
        private Collider _lastCollision;
        private RaycastHit[] _groundHits;

        public CapsuleCollider Collider { get; }
        public bool IsGrounded { get; private set; }
        public Vector3 Normal { get; private set; }
        public float Radius { get; set; }
        public LayerMask LayerMask { get; set; }
        public Vector3 Offset { get; set; }

        public CapsuleGroundChecker(CapsuleCollider collider, LayerMask layerMask)
        {
            Collider = collider;
            Radius = collider.radius + 0.01f;
            LayerMask = layerMask;
        }

        public void Check()
        {
            if (Physics.OverlapSphereNonAlloc(Collider.transform.position + Collider.center + Vector3.down * (Collider.height / 2 - Collider.radius), Radius, _groundColliders, LayerMask) > 0)
            {
                IsGrounded = true;

                if (_lastCollision == null)
                    OnGrounded?.Invoke();

                _lastCollision = _groundColliders[0];

                if (Physics.SphereCastNonAlloc(Collider.transform.position + Collider.center, Collider.radius, Vector3.down, _groundHits, Collider.height, LayerMask) > 0)
                {
                    Normal = _groundHits[0].normal;
                }
                return;
            }

            _lastCollision = null;
            IsGrounded = false;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Collider.transform.position + Collider.center + Vector3.down * (Collider.height / 2 - Collider.radius), Radius);
        }
    }
}
