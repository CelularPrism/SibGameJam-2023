using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private EndGameEvent eventor;
    
    private DateTime _start;
    private bool _timeStopped = false;

    void Start()
    {
        _start = DateTime.Now;
    }

    private void OnEnable()
    {
        GameEvents.Instance.Subscribe(GameEventType.Lose, eventor.Lose);
    }

    void FixedUpdate()
    {
        if (!_timeStopped)
        {
            var seconds = _start.Subtract(DateTime.Now.AddMinutes(-range)).TotalSeconds;
            if (seconds < 0)
            {
                GameEvents.Instance.Dispatch(GameEventType.Lose);
            }
        }
    }

    private void OnDisable()
    {
        GameEvents.Instance.UnSubscribe(GameEventType.Lose, eventor.Lose);
    }

    public void StopTimer()
    {
        _timeStopped = true;
    }

    public TimeSpan GetTime() => _start.Subtract(DateTime.Now.AddMinutes(-range));
}
