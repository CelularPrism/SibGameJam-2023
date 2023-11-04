using Scripts.GameEventsHolding;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class GameEvents : MonoBehaviour, IEventBus
{
    private Dictionary<GameEventType, UnityEvent> _events;

    public static GameEvents Instance { get; private set; }

    private void Awake()
    {
        _events = Enum
            .GetValues(typeof(GameEventType))
            .Cast<GameEventType>()
            .ToDictionary(gameEventType => gameEventType, gameEvent => new UnityEvent());

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    public void Subscribe(GameEventType eventType, params UnityAction[] actions)
    {
        if (_events.ContainsKey(eventType))
        {
            foreach (UnityAction action in actions)
            {
                _events[eventType].AddListener(action);
            }

            return;
        }

        Debug.LogWarning($"Failed to find {eventType} event");
    }

    public void Dispatch(GameEventType eventType)
    {
        if (_events.ContainsKey(eventType))
        {
            _events[eventType]?.Invoke();
        }
    }

    public void UnSubscribe(GameEventType eventType, params UnityAction[] actions)
    {
        if (_events.ContainsKey(eventType))
        {
            foreach (UnityAction action in actions)
            {
                _events[eventType].RemoveListener(action);
            }

            return;
        }

        Debug.LogWarning($"Failed to find {eventType} event");
    }

    public void Dispose()
    {
        _events.Values.ToList().ForEach(gameEvent => gameEvent.RemoveAllListeners());
        _events.Clear();
    }
}
