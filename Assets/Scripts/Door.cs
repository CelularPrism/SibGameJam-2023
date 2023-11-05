using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IItem
{
    [SerializeField] private GameObject ceiling;
    public void Use()
    {
        Debug.Log("Door destroyed");
        Destroy(ceiling);
        Destroy(transform.gameObject);
    }

}
