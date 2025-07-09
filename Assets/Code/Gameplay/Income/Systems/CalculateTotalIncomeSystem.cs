using Code.Gameplay.Income.Components;
using Code.Gameplay.LevelUp.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Income.Systems
{
    public class CalculateTotalIncomeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        
        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<BaseIncomeComponent>()
                .Inc<LevelComponent>()
                .Inc<TotalIncomeComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                var baseIncome = _filter.GetWorld().GetPool<BaseIncomeComponent>().Get(entity);
                ref var totalIncome = ref _filter.GetWorld().GetPool<TotalIncomeComponent>().Get(entity);
                var level = _filter.GetWorld().GetPool<LevelComponent>().Get(entity);

                totalIncome.Value = baseIncome.Value * level.Value;
            }
        }
    }
}