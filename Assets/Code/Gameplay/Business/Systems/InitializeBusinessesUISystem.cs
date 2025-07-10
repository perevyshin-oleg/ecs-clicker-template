using Code.Gameplay.Balance.Components;
using Code.Gameplay.Balance.Services;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Services;
using Code.Gameplay.BusinessUpgrades.Components;
using Code.Gameplay.BusinessUpgrades.Services;
using Code.Gameplay.BusinessUpgrades.UI;
using Code.Gameplay.Income.Components;
using Code.Gameplay.LevelUp.Components;
using Code.Gameplay.UI;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Gameplay.Business.Systems
{
    public class InitializeBusinessesUISystem : IEcsInitSystem, IEcsDestroySystem
    {
        private const string ScreenTag = "BusinessScreen";
        
        private readonly IBusinessesService _businessesService;
        private readonly IUserBalanceService _userBalanceService;
        private readonly IBusinessElementFactory _businessElementFactory;
        private readonly IUpgradesService _upgrades;
        private BusinessScreenController _screenController;

        public InitializeBusinessesUISystem(
            IBusinessesService businessesService, 
            IUserBalanceService userBalanceService, 
            IBusinessElementFactory businessElementFactory, 
            IUpgradesService upgrades)
        {
            _businessesService = businessesService;
            _userBalanceService = userBalanceService;
            _businessElementFactory = businessElementFactory;
            _upgrades = upgrades;
        }
        
        public void Init(IEcsSystems systems)
        {
            BusinessScreenUIView view = GameObject.FindWithTag(ScreenTag).GetComponent<BusinessScreenUIView>();
            _screenController = new BusinessScreenController(view, _businessesService, _userBalanceService, _businessElementFactory);

            EcsFilter businesses = systems.GetWorld()
                .Filter<BusinessComponent>()
                .Inc<LevelComponent>()
                .Inc<TotalIncomeComponent>()
                .Inc<TotalCostComponent>()
                .End();

            EcsFilter upgrades = systems.GetWorld()
                .Filter<BusinessUpgradeComponent>()
                .Inc<TotalCostComponent>()
                .Inc<IncomeModifierComponent>()
                .End();

            foreach (int entity in upgrades)
            {
                var upgrade = systems.GetWorld().GetPool<BusinessUpgradeComponent>().Get(entity);
                var cost = systems.GetWorld().GetPool<TotalCostComponent>().Get(entity);
                var incomeModifier = systems.GetWorld().GetPool<IncomeModifierComponent>().Get(entity);
                
                bool isPurchased = systems.GetWorld().GetPool<PurchasedComponent>().Has(entity);
                
                UpgradeModel model = new UpgradeModel(
                    upgrade.UpgradeId, upgrade.BusinessId, upgrade.Name, incomeModifier.Percent, cost.Value, isPurchased);
                
                _upgrades.AddUpgrade(model);
            }
            
            foreach (int entity in businesses)
            {
                var business = systems.GetWorld().GetPool<BusinessComponent>().Get(entity);
                var level = systems.GetWorld().GetPool<LevelComponent>().Get(entity);
                var totalIncome = systems.GetWorld().GetPool<TotalIncomeComponent>().Get(entity);
                var totalCost = systems.GetWorld().GetPool<TotalCostComponent>().Get(entity);
                
                BusinessModel model = new BusinessModel(
                    business.BusinessId, business.Name, level.Value, totalCost.Value, totalIncome.Value);
                
                _businessesService.AddBusinessModel(model);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _screenController?.Dispose();
        }
    }
}