using Code.Gameplay.Balance.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Services;
using Code.Gameplay.Income.Components;
using Code.Gameplay.LevelUp.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Income.Systems
{
    public class RefreshBusinessUISystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IBusinessesService _businesses;
        private EcsFilter _filter;

        public RefreshBusinessUISystem(IBusinessesService businesses)
        {
            _businesses = businesses;
        }

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<IncomeProgressComponent>()
                .Inc<BusinessComponent>()
                .Inc<LevelComponent>()
                .Inc<TotalCostComponent>()
                .Inc<TotalIncomeComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                BusinessComponent business = _filter.GetWorld().GetPool<BusinessComponent>().Get(entity);
                LevelComponent level = _filter.GetWorld().GetPool<LevelComponent>().Get(entity);
                TotalCostComponent cost = _filter.GetWorld().GetPool<TotalCostComponent>().Get(entity);
                IncomeProgressComponent incomeProgress = _filter.GetWorld().GetPool<IncomeProgressComponent>().Get(entity);
                TotalIncomeComponent income = _filter.GetWorld().GetPool<TotalIncomeComponent>().Get(entity);

                if (_businesses.CurrentBusinesses.TryGetValue(business.BusinessId, out BusinessModel model))
                {
                    model.SetIncomeProgress(incomeProgress.Progress);
                    model.SetLevel(level.Value);
                    model.SetTotalCost(cost.Value);
                    model.SetTotalIncome(income.Value);
                }
            }
        }
    }
}