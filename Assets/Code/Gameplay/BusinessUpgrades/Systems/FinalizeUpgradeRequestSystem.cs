using Code.Gameplay.BusinessUpgrades.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.BusinessUpgrades.Systems
{
    public class FinalizeUpgradeRequestSystem : IEcsInitSystem ,IEcsRunSystem
    {
        private EcsFilter _requests;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _requests = world.Filter<BusinessUpgradeRequest>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _requests)
            {
                systems.GetWorld().DelEntity(requestEntity);
            }
        }
    }
}