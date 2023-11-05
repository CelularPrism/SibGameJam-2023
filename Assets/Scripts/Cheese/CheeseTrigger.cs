using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseTrigger : MonoBehaviour, IItem
{
    [SerializeField] private CheeseInventory inventory;

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
