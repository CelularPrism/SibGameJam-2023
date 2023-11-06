using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Learn
{
    public class Introduce : MonoBehaviour
    {
        [SerializeField] private float _seconds;
        [SerializeField] private float _fadeSpeed;
        private WaitForSeconds _waitForSeconds;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _waitForSeconds = new(_seconds);
            _canvasGroup = GetComponent<CanvasGroup>();
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

            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= _fadeSpeed * Time.deltaTime;
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}
