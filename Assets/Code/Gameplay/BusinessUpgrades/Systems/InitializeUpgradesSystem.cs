using Code.Gameplay.Balance.Components;
using Code.Gameplay.Business.StaticData;
using Code.Gameplay.BusinessUpgrades.Components;
using Code.Gameplay.Income.Components;
using Code.Infrastructure.StaticDataProviders;
using Leopotam.EcsLite;

namespace Code.Gameplay.BusinessUpgrades.Systems
{
    public class InitializeUpgradesSystem : IEcsInitSystem
    {
        private readonly IStaticDataProvider _staticDataProvider;

        public InitializeUpgradesSystem(IStaticDataProvider staticDataProvider)
        {
            _staticDataProvider = staticDataProvider;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            GameStaticData gameData = _staticDataProvider.GameStaticData;

            int upgradeIndex = 0;
            
            for (var businessIndex = 0; businessIndex < gameData.Businesses.Count; businessIndex++)
            {
                BusinessStaticData businessData = gameData.Businesses[businessIndex];
                
                foreach (var upgradeData in businessData.Upgrades)
                {
                    int entity = world.NewEntity();
                    
                    ref TotalCostComponent cost = ref world.GetPool<TotalCostComponent>().Add(entity);
                    cost.Value = upgradeData.Cost;
                    
                    ref IncomeModifierComponent incomeModifier = ref world.GetPool<IncomeModifierComponent>().Add(entity);
                    incomeModifier.Percent = upgradeData.IncomeModificator;
                    
                    ref BusinessUpgradeComponent upgrade = ref world.GetPool<BusinessUpgradeComponent>().Add(entity);
                    upgrade.UpgradeId = upgradeIndex;
                    upgrade.BusinessId = businessIndex;
                    upgrade.Name = _staticDataProvider.GetNameByKey(upgradeData.NameKey);
                    
                    upgradeIndex++;
                }
            }
        }
    }
}
