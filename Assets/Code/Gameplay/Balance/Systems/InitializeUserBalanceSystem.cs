using Code.Gameplay.Balance.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Balance.Systems
{
    public class InitializeUserBalanceSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            int businessEntity = world.NewEntity();
            world.GetPool<UserBalanceComponent>().Add(businessEntity);
        }
    }
}
