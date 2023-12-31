using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour, IUsable
{
    [SerializeField] private HealthSystem healthSystem;

    private void OnTriggerEnter(Collider other)
    {
        var localHealthSystem = other.GetComponent<HealthSystem>();
        if (healthSystem == null && localHealthSystem != null)
            healthSystem = localHealthSystem;
    }

    public void Use()
    {
        if (healthSystem != null && !healthSystem.IsMax())
        {
            healthSystem.Heal(1);
            Debug.Log("Heal");
            gameObject.SetActive(false);
        }
    }
}
