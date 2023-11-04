using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseInventory : MonoBehaviour
{
    [SerializeField] private int max = 5;
    private int _count = 0;

    public void AddCheese()
    {
        _count++;
    }

    public int RemoveCheese()
    {
        var count = _count;
        _count = 0;
        return count;
    }

    public int GetCount() => _count;
    public int GetMaxCount() => max;

    public bool MaxCount() => _count >= max;
}
