using Code.Gameplay.Business.Components;
using Code.Gameplay.LevelUp.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class MarkBusinessPurchasedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<LevelComponent>()
                .Inc<BusinessComponent>()
                .Exc<PurchasedComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                LevelComponent level = systems.GetWorld().GetPool<LevelComponent>().Get(entity);

                if (level.Value > 0)
                {
                    systems.GetWorld().GetPool<PurchasedComponent>().Add(entity);
                }
            }
        }
    }
}