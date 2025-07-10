using System;
using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.BusinessUpgrades.Components;
using Code.Gameplay.BusinessUpgrades.UI;
using Leopotam.EcsLite;

namespace Code.Gameplay.BusinessUpgrades.Services
{
    public class UpgradesService : IUpgradesService
    {
        private readonly EcsWorld _world;
        
        public List<UpgradeModel> Upgrades { get; private set; }

        public event Action<UpgradeModel> OnUpgradePurchased;
        
        public UpgradesService(EcsWorld world)
        {
            _world = world;
            Upgrades = new List<UpgradeModel>();
        }

        public void AddUpgrade(UpgradeModel upgrade)
        {
            Upgrades.Add(upgrade);
        }
        
        public void PurchaseUpgrade(int upgradeId)
        {
            UpgradeModel upgrade = Upgrades.FirstOrDefault(u => u.Id == upgradeId);
            
            if (upgrade != null)
            {
                upgrade.SetPurchased();
                OnUpgradePurchased?.Invoke(upgrade);
            }
        }
            
        public void CreateUpgradeRequest(int upgradeId)
        {
            int entity = _world.NewEntity();
            ref var upgradeRequest = ref _world.GetPool<BusinessUpgradeRequest>().Add(entity);
            upgradeRequest.UpgradeId = upgradeId;
        }
    }
}
