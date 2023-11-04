using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gameplay_UI
{
    public class HealthBar : MonoBehaviour
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponentInChildren<Image>();
        }

        public void Set(int value)
        {
            for (int i = 0; i < value; i++)
            {
                GameObject heart;

                if (transform.childCount < i + 1)
                {
                    heart = Instantiate(_image, transform).gameObject;
                    heart.name = "Heart";
                }
                else
                {
                    heart = transform.GetChild(i).gameObject;
                }

                heart.SetActive(true);
            }

            if (value < transform.childCount)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (i > value - 1)
                        transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
