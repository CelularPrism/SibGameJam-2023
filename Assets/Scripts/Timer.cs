using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float range;
    private DateTime _start;

    void Start()
    {
        _start = DateTime.Now;
    }

    //void FixedUpdate()
    //{
    //    var time = GetTime();
    //    Debug.Log(time.Minutes + ":" + time.Seconds);
    //}

    public TimeSpan GetTime() => _start.Subtract(DateTime.Now.AddMinutes(-range));
}
