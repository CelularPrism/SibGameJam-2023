using Assets.Scripts.Gameplay_UI;
using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float range;
    [SerializeField] private EndGameEvent eventor;
    
    private DateTime _start;
    private DateTime _stopTime;
    private bool _timeStopped = false;
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
        GameEvents.Instance.Subscribe(GameEventType.Lose, eventor.Lose);
    }

    private void Update()
    {
        _ui.Set(GetTime());
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
        _stopTime = DateTime.Now;
    }

    public void StartTimer()
    {
        _timeStopped = false;
        range += DateTime.Now.Subtract(_stopTime).Seconds / 60.0f;
    }

    public TimeSpan GetTime() => _start.Subtract(DateTime.Now.AddMinutes(-range));
}
