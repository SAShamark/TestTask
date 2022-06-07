using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Clicks
{
    public class ButtonClickUpdate : MonoBehaviour
    {
        [SerializeField] private Button _updateButton;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private TMP_Text _number;
        public TMP_Text Level =>_level;
        public TMP_Text Price =>_price;
        public TMP_Text Number =>_number;
        public event Action OnClickUpdate;
        
        public void Initialize()
        {
            _updateButton.onClick.AddListener(Click);
        }

        private void Click()
        {
            OnClickUpdate?.Invoke();
        }

        private void OnDestroy()
        {
            _updateButton.onClick.RemoveListener(Click);
        }
    }
}
