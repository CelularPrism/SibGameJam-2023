using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Cheese
{
    public class CheeseBar : MonoBehaviour
    {
        private Canvas _canvas;
        private Image _image;
        private Camera _camera;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _image = GetComponent<Image>();
            _camera = Camera.main;
        }

        public void Set(float value) => _image.fillAmount = value;

        private void Update()
        {
            if (_camera)
            {
                _canvas.transform.LookAt(_camera.transform.position);
            }
        }
    }
}