using Code.Gameplay.Business.Components;
using Code.Gameplay.Income.Components;
using Code.Gameplay.LevelUp.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Income.Systems
{
    public class CalculateTotalIncomeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsFilter _modifiers;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<BaseIncomeComponent>()
                .Inc<BusinessComponent>()
                .Inc<LevelComponent>()
                .Inc<TotalIncomeComponent>()
                .Inc<ComposedIncomeModifier>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref var totalIncome = ref _filter.GetWorld().GetPool<TotalIncomeComponent>().Get(entity);
                var baseIncome = _filter.GetWorld().GetPool<BaseIncomeComponent>().Get(entity);
                var level = _filter.GetWorld().GetPool<LevelComponent>().Get(entity);
                var modifier = _filter.GetWorld().GetPool<ComposedIncomeModifier>().Get(entity);

                totalIncome.Value = (int)(baseIncome.Value * level.Value * ((100f + modifier.Percent) / 100f));
            }
        }
    }
}