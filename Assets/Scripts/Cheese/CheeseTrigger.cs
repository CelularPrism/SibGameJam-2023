using UnityEngine;

[RequireComponent (typeof(CheeseInstance))]
public class CheeseTrigger : MonoBehaviour, IUsable
{
    private CheeseInventory _inventory;
    private CheeseInstance _instance;

    private void Awake()
    {
        _instance = GetComponent<CheeseInstance>();
        _inventory = FindObjectOfType<CheeseInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var localInventory = other.GetComponent<CheeseInventory>();
        if (_inventory == null && localInventory != null)
            _inventory = localInventory;
    }

    public void Use()
    {
        if (_inventory.TryPut(_instance))
        {
            gameObject.SetActive(false);
        }
    }
}
