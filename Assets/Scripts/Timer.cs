using Assets.Scripts.Gameplay_UI;
using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float range;
    [SerializeField] private EndGameEvent eventor;
    
    private DateTime _start;
    private DateTime _stopTime;
    private UITimer _ui;

    private void Awake()
    {
        _ui = FindAnyObjectByType<UITimer>();
    }

    void Start()
    {
        _start = DateTime.Now;
    }

    private void OnEnable()
    {
        _ui.Set(GetTime());
        GameEvents.Instance.Subscribe(GameEventType.Lose, eventor.Lose);
    }

    private void FixedUpdate()
    {
        double seconds = _start.Subtract(DateTime.Now.AddMinutes(-range)).TotalSeconds;

        if (seconds < 0)
        {
            GameEvents.Instance.Dispatch(GameEventType.Lose);
        }

        _ui.Set(GetTime());
    }

    public TimeSpan GetTime() => _start.Subtract(DateTime.Now.AddMinutes(-range));

    private void OnDisable()
    {
        _stopTime = DateTime.Now;
        GameEvents.Instance.UnSubscribe(GameEventType.Lose, eventor.Lose);
    }
}
