using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Fire
{
    public class FireMediator : MonoBehaviour
    {
        private readonly List<FireInstance> _instances = new();
        private readonly List<FireInstance> _disabledInstances = new();

        [field: SerializeField] public int Limit { get; private set; } = 100;
        public bool IsLimitReached => _instances.Count >= Limit;

        private void Start()
        {
            int fireCount = FindObjectsOfType<FireInstance>().Length;
            Limit = fireCount > Limit ? fireCount : Limit;
        }

        public void Add(FireInstance instance) => _instances.Add(instance);

        [ContextMenu("Enable")]
        public void Enable()
        {
            if (_disabledInstances.Count == 0)
                return;

            _disabledInstances.ForEach(instance => instance.gameObject.SetActive(true));
            _disabledInstances.Clear();
        }

        [ContextMenu("Enable All")]
        public void EnableAll()
        {
            _instances.ForEach(instance => instance.gameObject.SetActive(true));
        }

        [ContextMenu("Disable")]
        public void Disable()
        {
            if (_disabledInstances.Count > 0)
                return;

            List<FireInstance> toDisables = _instances.Where(instance => instance.gameObject.activeInHierarchy).ToList();
            toDisables.ForEach(instance => instance.gameObject.SetActive(false));
            _disabledInstances.AddRange(toDisables);
        }
    }
}