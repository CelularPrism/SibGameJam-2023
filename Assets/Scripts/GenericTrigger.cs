using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class GenericTrigger : MonoBehaviour
    {
        public UnityEvent OnEnter, OnStay, OnExit;

        [SerializeField] private LayerMask _layerMask;

        private void OnTriggerEnter(Collider other)
        {
            if (_layerMask == (_layerMask | (1 << other.gameObject.layer)))
                OnEnter?.Invoke();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_layerMask == (_layerMask | (1 << collision.gameObject.layer)))
                OnEnter?.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            if (_layerMask == (_layerMask | (1 << other.gameObject.layer)))
                OnStay?.Invoke();
        }

        private void OnCollisionStay(Collision collision)
        {
            if (_layerMask == (_layerMask | (1 << collision.gameObject.layer)))
                OnStay?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (_layerMask == (_layerMask | (1 << other.gameObject.layer)))
                OnExit?.Invoke();
        }

        private void OnCollisionExit(Collision collision)
        {
            if (_layerMask == (_layerMask | (1 << collision.gameObject.layer)))
                OnExit?.Invoke();
        }
    }
}