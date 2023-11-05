using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour, IItem
{
    [SerializeField] private HealthSystem healthSystem;

    public void Use()
    {
        if (healthSystem != null)
        {
            healthSystem.Heal(1);
            Debug.Log("Heal");
            Destroy(transform.gameObject);
        }
    }
}
