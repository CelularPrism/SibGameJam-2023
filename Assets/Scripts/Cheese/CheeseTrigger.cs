using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseTrigger : MonoBehaviour, IItem
{
    [SerializeField] private CheeseInventory inventory;

    private void OnTriggerEnter(Collider other)
    {
        var localInventory = other.GetComponent<CheeseInventory>();
        if (inventory == null && localInventory != null)
            inventory = localInventory;
    }

    public void Use()
    {
        if (inventory != null && !inventory.MaxCount())
        {
            inventory.AddCheese();
            Debug.Log("Cheese added");
            Destroy(transform.gameObject);
        }
    }
}
