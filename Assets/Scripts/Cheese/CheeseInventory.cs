using System;
using UnityEngine;

public class CheeseInventory : MonoBehaviour
{
    [SerializeField, Range(0, 8)] private int max = 5;
    [SerializeField] private float _speedDecrease;
    [SerializeField] private Transform _view;
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
        DrawView();
    }

    private void DrawView()
    {
        for (int i = 0; i < _view.childCount; i++)
        {
            _view.GetChild(i).gameObject.SetActive(i < _count);
        }
    }

    public int RemoveCheese()
    {
        var count = _count;
        _count = 0;
        _motionController.MoveSpeed = _defaultSpeed;
        DrawView();
        return count;
    }

    public int GetCount() => _count;
    public int GetMaxCount() => max;

    public bool MaxCount() => _count >= max;
}
