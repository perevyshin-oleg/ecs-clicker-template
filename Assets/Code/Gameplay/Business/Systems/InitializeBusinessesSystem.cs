using Code.Gameplay.Balance.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.StaticData;
using Code.Gameplay.Income.Components;
using Code.Gameplay.LevelUp.Components;
using Code.Infrastructure.StaticDataProviders;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class InitializeBusinessesSystem : IEcsInitSystem
    {
        private readonly IStaticDataProvider _staticDataProvider;

        public InitializeBusinessesSystem(IStaticDataProvider staticDataProvider)
        {
            _staticDataProvider = staticDataProvider;
        }
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            GameStaticData gameData = _staticDataProvider.GameStaticData;
            
            for (int index = 0; index < gameData.Businesses.Count; index++)
            {
                BusinessStaticData businessData = gameData.Businesses[index];
                
                int businessEntity = world.NewEntity();
                
                ref LevelComponent level = ref world.GetPool<LevelComponent>().Add(businessEntity);
                level.Value = businessData.InitialLevel;
                
                ref BusinessComponent business = ref world.GetPool<BusinessComponent>().Add(businessEntity);
                business.Name = _staticDataProvider.GetNameByKey(businessData.NameKey);
                business.BusinessId = index;
                
                ref BaseIncomeComponent baseIncome = ref world.GetPool<BaseIncomeComponent>().Add(businessEntity);
                baseIncome.Value = businessData.BaseIncome;
                
                ref BaseCostComponent baseCost = ref world.GetPool<BaseCostComponent>().Add(businessEntity);
                baseCost.Value = businessData.BaseCost;
                
                ref IncomeProgressComponent progress = ref world.GetPool<IncomeProgressComponent>().Add(businessEntity);
                progress.Progress = 0;
                progress.Duration = businessData.DurationInSeconds;
                
                world.GetPool<ComposedIncomeModifier>().Add(businessEntity);
                world.GetPool<TotalIncomeComponent>().Add(businessEntity);
                world.GetPool<TotalCostComponent>().Add(businessEntity);
            }
        }
    }
}
