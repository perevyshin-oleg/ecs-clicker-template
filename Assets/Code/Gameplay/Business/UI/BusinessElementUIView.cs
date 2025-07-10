using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.UI
{
    public class BusinessElementUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _IncomeText;
        [SerializeField] private TextMeshProUGUI _LevelUpText;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private Button _levelUpButton;
        
        [field: SerializeField] public RectTransform UpgradesContainer { get;private set; }

        public event Action OnLevelUpClicked;

        public void SetName(string name) =>
            _nameText.text = name;

        public void SetLevel(string level) =>
            _levelText.text = level.ToString();

        public void SetIncome(string income) =>
            _IncomeText.text = income.ToString();

        public void SetProgressValue(float progress) =>
            _progressBar.value = progress;

        public void SetLevelUpText(string text) =>
            _LevelUpText.text = text;

        private void OnEnable()
        {
            _levelUpButton.onClick.AddListener(OnLevelUpClick);
        }

        private void OnDisable()
        {
            _levelUpButton.onClick.RemoveListener(OnLevelUpClick);
        }

        private void OnLevelUpClick()
        {
            OnLevelUpClicked?.Invoke();
        }
    }
}