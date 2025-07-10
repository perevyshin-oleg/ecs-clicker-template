using System;
using System.Collections.Generic;
using Code.Gameplay.BusinessUpgrades.UI;

namespace Code.Gameplay.BusinessUpgrades.Services
{
    public interface IUpgradesService
    {
        void CreateUpgradeRequest(int upgradeId);
        event Action<UpgradeModel> OnUpgradePurchased;
        List<UpgradeModel> Upgrades { get; }
        void PurchaseUpgrade(int upgradeId);
        void AddUpgrade(UpgradeModel upgrade);
    }
}