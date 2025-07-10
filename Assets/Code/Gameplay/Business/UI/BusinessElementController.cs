using System;
using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Business.Services;
using Code.Gameplay.BusinessUpgrades.Services;
using Code.Gameplay.BusinessUpgrades.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Gameplay.UI
{
    public class BusinessElementController : IDisposable
    {
        private const string UpgradeButtonPath = "Prefabs/UpgradeButton";
        
        private readonly BusinessElementUIView _view;
        private readonly BusinessModel _model;
        private readonly IBusinessesService _businessesService;
        private readonly IUpgradesService _upgradesService;
        private readonly Dictionary<int, UpgradeButton> _upgradeButtons;

        public BusinessElementController(
            BusinessElementUIView view,
            IBusinessesService businessesService,
            BusinessModel businessModel, 
            IUpgradesService upgradesService)
        {
            _upgradeButtons = new Dictionary<int, UpgradeButton>();
            _view = view;
            _businessesService = businessesService;
            _model = businessModel;
            _upgradesService = upgradesService;
        }

        public void Initialize()
        {
            UpdateView();

            List<UpgradeModel> upgrades = _upgradesService.Upgrades
                .Where(u => u.BusinessId == _model.Id)
                .ToList();

            foreach (UpgradeModel upgrade in upgrades)
            {
                AddUpgradeButton(upgrade);
            }
            
            _model.Updated += UpdateView;
            _view.OnLevelUpClicked += OnLevelUpClick;
            _upgradesService.OnUpgradePurchased += UpdateUpgradesView;
        }

        private void AddUpgradeButton(UpgradeModel upgrade)
        {
            GameObject resource = Resources.Load<GameObject>(UpgradeButtonPath);
            GameObject obj = Object.Instantiate(resource, _view.UpgradesContainer);
            UpgradeButton button = obj.GetComponent<UpgradeButton>();
            UpdateButtonText(button, upgrade);
            button.OnClick += () => OnUpgradeClick(upgrade.Id);
            button.gameObject.SetActive(true);
            _upgradeButtons.Add(upgrade.Id ,button);
        }

        private void UpdateUpgradesView(UpgradeModel upgrade)
        {
            if (_upgradeButtons.TryGetValue(upgrade.Id, out UpgradeButton button))
            {
                UpdateButtonText(button, upgrade);
            }
        }

        private void UpdateButtonText(UpgradeButton button, UpgradeModel upgrade)
        {
            button.SetText($"{upgrade.Name}" +
                           $"\n+{upgrade.IncomeModifier}%" +
                           $"\n{(upgrade.IsPurchased ? "Куплено" : $"Цена: {upgrade.Cost}$")}");
        }

        private void UpdateView()
        {
            _view.SetName(_model.Name);
            _view.SetLevel($"LVL\n{_model.Level}");
            _view.SetIncome($"Доход\n{_model.TotalIncome}");
            _view.SetLevelUpText($"LVL UP\n{_model.TotalCost}");
            _view.SetProgressValue(_model.IncomeProgress);
        }

        private void OnUpgradeClick(int upgradeId)
        {
            UpgradeModel upgrade = _upgradesService.Upgrades.FirstOrDefault(u => u.Id == upgradeId);

            if (upgrade is { IsPurchased: false })
            {
                _upgradesService.CreateUpgradeRequest(upgradeId);
            }
        }

        private void OnLevelUpClick() =>
            _businessesService.CreateLevelUpRequest(_model.Id);

        public void Dispose()
        {
            _view.OnLevelUpClicked -= OnLevelUpClick;
        }
    }
}