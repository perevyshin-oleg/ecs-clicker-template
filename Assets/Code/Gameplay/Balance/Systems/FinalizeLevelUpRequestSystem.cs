using Code.Gameplay.LevelUp.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Balance.Systems
{
    public class FinalizeLevelUpRequestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _requests;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _requests = world.Filter<LevelUpRequest>().End();
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