using TMPro;
using UnityEngine;

namespace Assets.Scripts.Gameplay_UI
{
    public class CheeseCounter : MonoBehaviour
    {
        private TMP_Text _count;

        private void Awake()
        {
            _count = GetComponentInChildren<TMP_Text>();
        }

        public void Set(int count, int outOf) => _count.text = $"{count}/{outOf}";
    }
}
