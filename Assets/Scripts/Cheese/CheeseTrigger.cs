using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CheeseInventory>() != null)
        {
            var inventory = other.GetComponent<CheeseInventory>();
            if (!inventory.MaxCount())
            {
                inventory.AddCheese();
                Destroy(transform.gameObject);
            }
        }
    }
}
