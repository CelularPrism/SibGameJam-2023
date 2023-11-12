using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class GenericTrigger : MonoBehaviour
    {
        public UnityEvent OnEnter, OnStay, OnExit;

        private void OnTriggerEnter(Collider other) => OnEnter?.Invoke();

        private void OnCollisionEnter(Collision collision) => OnEnter?.Invoke();

        private void OnTriggerStay(Collider other) => OnStay?.Invoke();

        private void OnCollisionStay(Collision collision) => OnStay?.Invoke();

        private void OnTriggerExit(Collider other) => OnExit?.Invoke();

        private void OnCollisionExit(Collision collision) => OnExit?.Invoke();
    }
}