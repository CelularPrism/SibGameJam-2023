using System;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class HurtBox<C> : MonoBehaviour where C : Collider
    {
        public event Action<Collider> OnEnter;
        public event Action<Collider> OnStay;
        public event Action<Collider> OnExit;

        public C Collider { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            Collider ??= GetComponent<C>();
            OnEnter?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            Collider ??= GetComponent<C>();
            OnStay?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            Collider ??= GetComponent<C>();
            OnExit?.Invoke(other);
        }
    }
}