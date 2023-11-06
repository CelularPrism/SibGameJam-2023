using Assets.Scripts.Gameplay_UI;
using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private EndGameEvent eventor;
    
    private DateTime _start;
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
    }

    public void StartTimer()
    {
        _timeStopped = false;
    }

    public TimeSpan GetTime() => _start.Subtract(DateTime.Now.AddMinutes(-range));
}
