using Code.Gameplay.Business.Components;
using Code.Gameplay.Income.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Gameplay.Income.Systems
{
    public class IncreaseIncomeProgressSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld()
                .Filter<IncomeProgressComponent>()
                .Inc<PurchasedComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref IncomeProgressComponent incomeProgress = ref _filter
                    .GetWorld().GetPool<IncomeProgressComponent>().Get(entity);

                incomeProgress.Progress += 1f / incomeProgress.Duration * Time.deltaTime;

                if (incomeProgress.Progress >= 1f)
                {
                    incomeProgress.Progress -= 1f;
                    systems.GetWorld().GetPool<IncomeRequest>().Add(entity);
                }
            }
        }
    }
}
