using Assets.Scripts.Minigames;
using Assets.Scripts.Tears;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Obstacles
{
    public class Obstacle : MonoBehaviour, IUsable
    {
        public UnityEvent OnComplete;
        public UnityEvent OnLose;
        public UnityEvent OnEscape;

        [SerializeField] private Minigame _minigame;
        private RBMotionController _motionController;
        private TearsSource _tears;

        private void Awake()
        {
            _motionController = FindObjectOfType<RBMotionController>();
            _tears = FindObjectOfType<TearsSource>();
        }

        public void Use()
        {
            if (_minigame.gameObject.activeInHierarchy == false)
            {
                _motionController.enabled = false;
                _tears.enabled = false;
                _minigame.Launch(OnCompleted, OnLost, OnEscaped);
            }
        }

        private void OnCompleted()
        {
            _motionController.enabled = true;
            _tears.enabled = true;
            OnComplete?.Invoke();
        }

        private void OnLost()
        {
            _motionController.enabled = true;
            _tears.enabled = true;
            OnLose?.Invoke();
        }

        private void OnEscaped()
        {
            _motionController.enabled = true;
            _tears.enabled = true;
            OnEscape?.Invoke();
        }
    }
}