using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject learnObject;
    [SerializeField] private GameObject[] objects;

    private void OnTriggerEnter(Collider other)
    {
        var character = other.GetComponent<PlayerCharacter>();
        if (character != null)
        {
            if (learnObject != null)
                learnObject.SetActive(true);
            foreach (var obj in objects)
            {
                obj.SetActive(true);
            }
            Destroy(transform.gameObject);
        }
    }
}
