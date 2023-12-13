using System;
using UnityEngine;

namespace Assets.Scripts.Minigames
{
    public abstract class Minigame : MonoBehaviour
    {
        public abstract void Lounch(Action onCompleted, Action onLost, Action onEscaped);
        public abstract void Complete();
        public abstract void Lose();
        public abstract void Escape();
    }
}