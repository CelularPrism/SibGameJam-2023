using UnityEngine;

public class CheeseInventory : MonoBehaviour
{
    [SerializeField] private int max = 5;
    [SerializeField] private float _speedDecrease;
    private int _count = 0;
    private CharacterMotionController _motionController;
    private float _defaultSpeed;

    private void Awake()
    {
        _motionController = GetComponent<CharacterMotionController>();
        _defaultSpeed = _motionController.MoveSpeed;
    }

    public void AddCheese()
    {
        _count++;
        _motionController.MoveSpeed -= _speedDecrease;
    }

    public int RemoveCheese()
    {
        var count = _count;
        _count = 0;
        _motionController.MoveSpeed = _defaultSpeed;
        return count;
    }

    public int GetCount() => _count;
    public int GetMaxCount() => max;

    public bool MaxCount() => _count >= max;
}
