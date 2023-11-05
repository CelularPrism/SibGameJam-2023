using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCheeseInventory : MonoBehaviour, IItem
{
    [SerializeField] private int maxCheese;
    [SerializeField] private GameEvents events;
    [SerializeField] private CheeseInventory inventory;
    private int _cheese = 0;

    public void Use()
    {
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
