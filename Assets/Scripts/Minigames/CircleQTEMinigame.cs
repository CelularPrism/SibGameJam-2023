using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Minigames
{
    public class CircleQTEMinigame : Minigame
    {
        [SerializeField] private AnimationCurve _dynamicsCurve;
        [SerializeField] private Vector2 _range = new(100, 250);
        [SerializeField] private float _speed;
        [SerializeField, Range(0, 1)] private float _accuracy = 0.5f;
        [SerializeField] private Image _static, _dynamic;
        private float _value;
        private Action _completeCallback, _loseCallback, _escapeCallback;
        private float _time;

        private void OnValidate()
        {
            if (_static && _dynamic)
            {
                _static.rectTransform.sizeDelta = Vector2.one * _range.y / 2;
                _dynamic.rectTransform.sizeDelta = Vector2.one * _range.y;
            }
        }

        public override void Launch(Action onCompleted, Action onLost, Action onEscaped)
        {
            _completeCallback = onCompleted;
            _loseCallback = onLost;
            _escapeCallback = onEscaped;
            _static.rectTransform.sizeDelta = GetRandomSize();
            _dynamic.rectTransform.sizeDelta = Vector2.one * _range.y;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            _time += Time.deltaTime * _speed;

            if (_time > 1.0f)
                _time = 0;

            _value = _dynamicsCurve.Evaluate(_time);
            Vector2 valueSize = _range.y * _value * Vector2.one;
            _dynamic.rectTransform.sizeDelta = valueSize;
            float staticMagnitude = _static.rectTransform.sizeDelta.magnitude;
            float dynamicMagnitude = _dynamic.rectTransform.sizeDelta.magnitude;


            if (Input.GetMouseButtonDown(0))
            {
                float difference = Mathf.Abs(staticMagnitude - dynamicMagnitude) * _accuracy;

                if (difference < 1)
                {
                    Complete();
                }
            }
        }

        public override void Complete()
        {
            gameObject.SetActive(false);
            _completeCallback?.Invoke();
        }

        public override void Lose()
        {
            gameObject.SetActive(false);
            _loseCallback?.Invoke();
        }

        public override void Escape()
        {
            gameObject.SetActive(false);
            _escapeCallback?.Invoke();
        }

        private Vector2 GetRandomSize()
        {
            float ramdomSize = UnityEngine.Random.Range(_range.x, _range.y);
            return new Vector2(ramdomSize, ramdomSize);
        }
    }
}