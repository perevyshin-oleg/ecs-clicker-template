using System;
using System.Collections.Generic;
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
        [SerializeField] private RectTransform _upgradeContainer;
        [SerializeField] private UpgradeButton _upgradeButtonPrefab;
        [SerializeField] private Button _levelUpButton;

        private readonly Dictionary<UpgradeButton, Action> _createdButtons = new();

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

        public void AddUpgradeButton(Action onClickEvent)
        {
            UpgradeButton button = Instantiate(_upgradeButtonPrefab, _upgradeContainer);
            button.gameObject.SetActive(true);
            button.OnClick += onClickEvent;
            _createdButtons.Add(button, onClickEvent);
        }

        private void OnEnable()
        {
            _levelUpButton.onClick.AddListener(OnLevelUpClick);
            foreach (var pair in _createdButtons)
            {
                if (pair.Key != null)
                {
                    pair.Key.OnClick += pair.Value;
                }
            }
        }

        private void OnDisable()
        {
            _levelUpButton.onClick.RemoveListener(OnLevelUpClick);
            foreach (var pair in _createdButtons)
            {
                if (pair.Key != null)
                {
                    pair.Key.OnClick -= pair.Value;
                }
            }
        }

        private void OnLevelUpClick()
        {
            OnLevelUpClicked?.Invoke();
        }
    }
}