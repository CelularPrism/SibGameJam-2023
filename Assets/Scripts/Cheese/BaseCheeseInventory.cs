using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCheeseInventory : MonoBehaviour
{
    [SerializeField] private int maxCheese;
    [SerializeField] private GameEvents events;
    private int _cheese = 0;

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.GetComponent<CheeseInventory>();
        if (inventory != null)
        {
            _cheese += inventory.RemoveCheese();
            if (_cheese >= maxCheese)
            {
                //var actions = new UnityAction[0];
                //events.Subscribe(GameEventType.Won, actions);
                Debug.Log("Won");
            }
        }
    }
}
