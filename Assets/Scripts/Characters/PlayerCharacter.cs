using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerCharacter : MonoBehaviour, ICharacter
{
    public bool IsDead { get; private set; }

    public void Dead()
    {
        Debug.Log("Dead");
    }
}
