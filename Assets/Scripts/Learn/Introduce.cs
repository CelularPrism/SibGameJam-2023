using Assets.Scripts.Tears;
using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Learn
{
    public class Introduce : MonoBehaviour
    {
        [SerializeField] private float _seconds;
        [SerializeField] private float _fadeSpeed;
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _cheese, _minutes;
        [SerializeField] private CinemachineVirtualCamera _mauseCamera;
        private WaitForSeconds _waitForSeconds;
        private CanvasGroup _canvasGroup;
        private Timer _timer;
        private BaseCheeseInventory _cheeseBox;
        private CharacterMotionController _characterController;
        private TearsSource _tearsSource;

        private void Awake()
        {
            _waitForSeconds = new(_seconds);
            _canvasGroup = GetComponent<CanvasGroup>();
            _timer = FindAnyObjectByType<Timer>(FindObjectsInactive.Include);
            _cheeseBox = FindAnyObjectByType<BaseCheeseInventory>();
            _characterController = FindObjectOfType<CharacterMotionController>();
            _tearsSource = FindObjectOfType<TearsSource>();
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void Update()
        {
            if (_cheese)
                _cheese.text = _cheeseBox.Target.ToString();

            _minutes.text = _timer.range.ToString();
        }

        private void OnCloseButtonClicked() => StartCoroutine(Desappear());

        private void Start()
        {
            if (_seconds > 0)
                StartCoroutine(Routine());
        }

        private IEnumerator Routine()
        {
            while (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += _fadeSpeed * Time.deltaTime;
                yield return null;
            }

            yield return _waitForSeconds;
            yield return Desappear();
        }

        private IEnumerator Desappear()
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= _fadeSpeed * Time.deltaTime;
                yield return null;
            }

            _timer.enabled = true;
            gameObject.SetActive(false);
            _characterController.enabled = true;
            _tearsSource.enabled = true;
            _mauseCamera.enabled = true;
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }
    }
}
