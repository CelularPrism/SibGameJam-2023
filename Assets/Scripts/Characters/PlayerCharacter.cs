using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerCharacter : MonoBehaviour, ICharacter
{
    [SerializeField] private EndGameEvent eventor;

    public bool IsDead { get; private set; }

    public void Dead()
    {
        GameEvents.Instance.Subscribe(GameEventType.Lose, eventor.Lose);
        GameEvents.Instance.Dispatch(GameEventType.Lose);
        Debug.Log("Dead");
    }

    private void OnDisable()
    {
        GameEvents.Instance.UnSubscribe(GameEventType.Lose, eventor.Lose);
    }
}
