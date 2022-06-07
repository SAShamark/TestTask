using System;
using UnityEngine;
using UnityEngine.UI;

namespace Clicks
{
    public class ClickOnPlayer : MonoBehaviour
    {
        [SerializeField] private Button _button;
        public event Action OnClick;

        public void Initialize()
        {
            _button.onClick.AddListener(Click);
        }

        private void Click()
        {
            OnClick?.Invoke();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(Click);
        }
    }
}