using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var healthSystem = other.GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.Heal(1);
            Debug.Log("Heal");
            Destroy(transform.gameObject);
        }
    }
}
