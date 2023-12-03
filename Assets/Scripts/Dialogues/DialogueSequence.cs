using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Dialogues
{
    public class DialogueSequence : MonoBehaviour
    {
        [SerializeField] private TMP_Text _field, _skipMessage;
        [SerializeField] private Transform _persons;
        [SerializeField] private Color _fadeColor;
        [SerializeField] private float _fadeSpeed, _textSpeed;
        [SerializeField] private DialogueMessage[] _messages;
        private int _messageIndex, _characterIndex;
        private float _fadeTime, _textTime;
        private bool _isTelling;
        private string _text;
        private Image[] _personsImages;
        private bool _isTextFinished;

        private void Awake()
        {
            _personsImages = _persons.GetComponentsInChildren<Image>(includeInactive: true);
        }

        private void OnEnable()
        {
            for (int i = 0; i < _personsImages.Length; i++)
            {
                _personsImages[i].color = _fadeColor;
                _personsImages[i].rectTransform.localScale = Vector3.one * 0.7f;
            }
            Tell();
        }

        private void Update()
        {
            if (_isTelling)
            {
                if (Input.GetMouseButtonDown(0))
                    Next();

                if (_isTextFinished == false)
                {
                    if (_characterIndex < _text.Length)
                    {
                        if (_textTime >= 1)
                        {
                            _field.text += _text[_characterIndex];
                            _textTime = 0;
                            _characterIndex++;
                        }

                        _textTime += _textSpeed * Time.deltaTime;
                    }
                    else
                    {
                        _skipMessage.gameObject.SetActive(true);
                        _isTextFinished = true;
                    }
                }

                if (_messageIndex == 0)
                    _fadeTime = 1;

                _fadeTime = Mathf.Clamp01(_fadeTime);

                if (_messageIndex > 0)
                    if (_messages[_messageIndex - 1].Person != _messages[_messageIndex].Person)
                    {
                        _messages[_messageIndex - 1].Person.color = Color
                            .Lerp(_messages[_messageIndex - 1].Person.color, _fadeColor, _fadeTime);

                        _messages[_messageIndex - 1].Person.rectTransform.localScale = Vector3
                            .Lerp(_messages[_messageIndex - 1].Person.rectTransform.localScale, Vector3.one * 0.7f, _fadeTime);
                    }

                _messages[_messageIndex].Person.color = Color
                    .Lerp(_messages[_messageIndex].Person.color, Color.white, _fadeTime);

                _messages[_messageIndex].Person.rectTransform.localScale = Vector3
                    .Lerp(_messages[_messageIndex].Person.rectTransform.localScale, Vector3.one, _fadeTime);

                _fadeTime += _fadeSpeed * Time.deltaTime;
            }
        }

        private void Tell()
        {
            _fadeTime = 0;
            _textTime = 0;
            _characterIndex = 0;
            _field.text = "";
            _text = _messages[_messageIndex].Text;
            _isTextFinished = false;
            _isTelling = true;

            if (_messages[_messageIndex].Emotion != null)
                _messages[_messageIndex].Person.sprite = _messages[_messageIndex].Emotion;
        }

        private void Next()
        {
            if (_messageIndex >= _messages.Length - 1)
            {
                _isTelling = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }

            if (_isTextFinished == false)
            {
                _skipMessage.gameObject.SetActive(true);
                _isTextFinished = true;
                _field.text = _text;
                return;
            }

            _skipMessage.gameObject.SetActive(false);
            _isTelling = false;
            _messageIndex++;

            Tell();
        }
    }

    [Serializable]
    public struct DialogueMessage
    {
        [field: SerializeField] public Image Person { get; private set; }
        [field: SerializeField] public Sprite Emotion { get; private set; }
        [field: SerializeField, TextArea(1, 100)] public string Text { get; private set; }
    }
}