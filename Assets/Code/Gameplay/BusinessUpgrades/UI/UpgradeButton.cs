using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.UI
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _upgradeButton;
            
        public event Action OnClick;

        public void SetText(string text) =>
            _text.text = text;
        
        private void OnEnable() =>
            _upgradeButton.onClick.AddListener(OnButtonClick);
        
        private void OnDisable() =>
            _upgradeButton.onClick.RemoveListener(OnButtonClick);
        
        private void OnButtonClick() =>
            OnClick?.Invoke();
    }
}