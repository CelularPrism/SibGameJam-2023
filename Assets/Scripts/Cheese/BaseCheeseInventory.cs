using Assets.Scripts.Gameplay_UI;
using UnityEngine;

public class BaseCheeseInventory : MonoBehaviour, IItem
{
    [SerializeField] private int maxCheese;
    [SerializeField] private CheeseInventory inventory;
    [SerializeField] private EndGameEvent eventor;

    private int _cheese = 0;
    private CheeseCounter _ui;

    private void Awake()
    {
        _ui = FindAnyObjectByType<CheeseCounter>();
    }

    public void Use()
    {
        if (inventory != null)
        {
            _cheese += inventory.RemoveCheese();
            _ui.Set(_cheese, maxCheese);

            if (_cheese >= maxCheese)
            {
                GameEvents.Instance.Subscribe(GameEventType.Won, eventor.Win);
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
