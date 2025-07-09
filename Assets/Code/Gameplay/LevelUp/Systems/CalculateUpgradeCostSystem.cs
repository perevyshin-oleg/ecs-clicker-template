using Code.Gameplay.Balance.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.LevelUp.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.LevelUp.Systems
{
    public class CalculateUpgradeCostSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<TotalCostComponent>()
                .Inc<BaseCostComponent>()
                .Inc<LevelComponent>()
                .Inc<BusinessComponent>()
                .End();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                BaseCostComponent baseCost = systems.GetWorld().GetPool<BaseCostComponent>().Get(entity);
                LevelComponent level = systems.GetWorld().GetPool<LevelComponent>().Get(entity);
                ref TotalCostComponent totalCost = ref systems.GetWorld().GetPool<TotalCostComponent>().Get(entity);
                
                totalCost.Value = baseCost.Value * (level.Value + 1);
            }
        }
    }
}
