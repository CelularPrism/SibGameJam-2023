using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCheeseInventory : MonoBehaviour, IItem
{
    [SerializeField] private int maxCheese;
    [SerializeField] private CheeseInventory inventory;
    [SerializeField] private EndGameEvent eventor;

    private int _cheese = 0;

    private void OnEnable()
    {
        GameEvents.Instance.Subscribe(GameEventType.Won, eventor.Win);
    }

    public void Use()
    {
        if (inventory != null)
        {
            _cheese += inventory.RemoveCheese();
            if (_cheese >= maxCheese)
            {
                GameEvents.Instance.Dispatch(GameEventType.Won);
                Debug.Log("Won");
            }
        }
    }

    private void OnDisable()
    {
        GameEvents.Instance.UnSubscribe(GameEventType.Won);
    }
}
