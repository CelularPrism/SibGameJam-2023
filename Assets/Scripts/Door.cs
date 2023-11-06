using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IItem
{
    [SerializeField] private GameObject ceiling;

    private void Start()
    {
        if (ceiling != null)
        {
            ceiling.SetActive(true);
        }
    }

    public void Use()
    {
        if (ceiling != null)
            ceiling.GetComponent<WallDestroyer>().Destroy();
        Destroy(transform.gameObject);
    }

}
