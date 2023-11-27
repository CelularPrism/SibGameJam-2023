using Assets.Scripts.Gameplay_UI;
using FMODUnity;
using UnityEngine;

public class BaseCheeseInventory : MonoBehaviour, IItem
{
    [SerializeField] private CheeseInventory _inventory;
    [SerializeField] private EndGameEvent _eventor;
    [SerializeField] private EventReference _useEvent;
    private CheeseCounter _ui;
    private CheeseInstance[] _allCheeseOnLevel;

    public int Current { get; private set; } = 0;
    public int Target { get; private set; }

    private void Awake()
    {
        _ui = FindAnyObjectByType<CheeseCounter>();
        _allCheeseOnLevel = FindObjectsOfType<CheeseInstance>();
        _inventory = FindObjectOfType<CheeseInventory>();

        foreach (CheeseInstance cheese in _allCheeseOnLevel)
        {
            Target += (int)(cheese.Size / 0.125f);
        }
    }

    private void Start()
    {
        _ui.Set(Current, Target);
    }

    private void OnTriggerEnter(Collider other)
    {
        var localInventory = other.GetComponent<CheeseInventory>();
        if (_inventory == null && localInventory != null)
            _inventory = localInventory;
    }

    public void Use()
    {
        if (_inventory != null)
        {
            Current += (int)(_inventory.RemoveAll() / 0.125f);
            _ui.Set(Current, Target);
            RuntimeManager.PlayOneShot(_useEvent, transform.position);

            if (Current >= Target)
            {
                GameEvents.Instance.Subscribe(GameEventType.Won, _eventor.Win);
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
