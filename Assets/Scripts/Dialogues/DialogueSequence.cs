using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Dialogues
{
    public class DialogueSequence : MonoBehaviour
    {
        [SerializeField] private TMP_Text _field;
        [SerializeField] private Image _leftPerson, _rightPerson;
        [SerializeField] private Button _nextButton;
        [SerializeField, Range(0, 1)] private float _fade;
        [SerializeField] private DialogueMessage[] _messages;
        private Color _fadedColor = Color.white;
        private int _index;

        private void Awake()
        {
            _fadedColor.a = _fade;
        }

        private void OnEnable()
        {
            Tell();
            _nextButton.onClick?.AddListener(Next);
        }

        private void Tell()
        {
            _leftPerson.color = _messages[_index].IsLeft ? Color.white: _fadedColor;
            _rightPerson.color = _messages[_index].IsLeft ? _fadedColor : Color.white;
            _field.text = _messages[_index].Text;
        }

        private void Next()
        {
            _index++;

            if (_index >= _messages.Length)
            {
                _nextButton.interactable = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }

            Tell();
        }

        private void OnDisable()
        {
            _nextButton.onClick?.RemoveListener(Next);
        }
    }

    [Serializable]
    public struct DialogueMessage
    {
        public bool IsLeft;
        [TextArea(1, 100)] public string Text;
    }
}