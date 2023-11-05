using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Gameplay_UI
{
    public class UITimer : MonoBehaviour
    {
        private TMP_Text _time;

        private void Awake()
        {
            _time = GetComponentInChildren<TMP_Text>();
        }

        public void Set(DateTime time) => _time.text = $"{time.Minute} : {time.Second}";
    }
}
