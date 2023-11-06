using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Learn
{
    public class Introduce : MonoBehaviour
    {
        [SerializeField] private float _seconds;
        [SerializeField] private float _fadeSpeed;
        [SerializeField] private TMP_Text _cheese, _minutes;
        private WaitForSeconds _waitForSeconds;
        private CanvasGroup _canvasGroup;
        private Timer _timer;
        private BaseCheeseInventory _cheeseBox;

        private void Awake()
        {
            _waitForSeconds = new(_seconds);
            _canvasGroup = GetComponent<CanvasGroup>();
            _timer = FindAnyObjectByType<Timer>();
            _cheeseBox = FindAnyObjectByType<BaseCheeseInventory>();
        }

        private void Start() => StartCoroutine(Routine());

        private IEnumerator Routine()
        {
            _timer.StopTimer();
            _cheese.text = _cheeseBox.maxCheese.ToString();
            _minutes.text = _timer.range.ToString();

            while (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += _fadeSpeed * Time.deltaTime;
                yield return null;
            }

            yield return _waitForSeconds;

            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= _fadeSpeed * Time.deltaTime;
                yield return null;
            }

            _timer.StartTimer();
            gameObject.SetActive(false);
        }
    }
}
