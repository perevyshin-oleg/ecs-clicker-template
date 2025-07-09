using System.Collections.Generic;
using Code.Gameplay.Balance.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.StaticData;
using Code.Gameplay.BusinessUpgrades.Components;
using Code.Gameplay.Income.Components;
using Code.Gameplay.LevelUp.Components;
using Code.Infrastructure.StaticDataProviders;
using Leopotam.EcsLite;
using Unity.VisualScripting;

namespace Code.Gameplay.Business.Systems
{
    public class InitializeBusinessesSystem : IEcsInitSystem
    {
        private readonly GameStaticData _gameData;

        public InitializeBusinessesSystem(GameStaticData gameData)
        {
            _gameData = gameData;
        }
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            for (var index = 0; index < _gameData.Businesses.Count; index++)
            {
                BusinessStaticData businessData = _gameData.Businesses[index];
                
                int businessEntity = world.NewEntity();
                
                ref LevelComponent level = ref world.GetPool<LevelComponent>().Add(businessEntity);
                level.Value = businessData.InitialLevel;
                
                ref BusinessComponent business = ref world.GetPool<BusinessComponent>().Add(businessEntity);
                business.Name = businessData.Name;
                business.BusinessId = index;
                
                ref BaseIncomeComponent baseIncome = ref world.GetPool<BaseIncomeComponent>().Add(businessEntity);
                baseIncome.Value = businessData.BaseIncome;
                
                ref BaseCostComponent baseCost = ref world.GetPool<BaseCostComponent>().Add(businessEntity);
                baseCost.Value = businessData.BaseCost;
                
                ref IncomeProgressComponent progress = ref world.GetPool<IncomeProgressComponent>().Add(businessEntity);
                progress.Progress = 0;
                progress.Duration = businessData.DurationInSeconds;
                
                ref BusinessUpgradesComponent upgrades = ref world.GetPool<BusinessUpgradesComponent>().Add(businessEntity);
                foreach (var upgradeData in businessData.Upgrades)
                {
                    upgrades.Upgrades = new List<BusinessUpgradesComponent.BusinessUpgrade>();
                    upgrades.Upgrades.Add(new ()
                    {
                        Name = upgradeData.Name,
                        Cost = upgradeData.BaseCost,
                        IsPurchased = false
                    });
                }
                
                world.GetPool<TotalIncomeComponent>().Add(businessEntity);
                world.GetPool<TotalCostComponent>().Add(businessEntity);
            }
        }
    }
}
