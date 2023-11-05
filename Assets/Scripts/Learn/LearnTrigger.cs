using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject learnObject;

    private void OnTriggerEnter(Collider other)
    {
        var healthSystem = other.GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            learnObject.SetActive(true);
            Destroy(transform.gameObject);
        }
    }
}
