using Code.Gameplay.Balance.Components;
using Code.Gameplay.Balance.Services;
using Leopotam.EcsLite;

namespace Code.Gameplay.Balance.Systems
{
    public class RefreshUserBalanceSystem : IEcsInitSystem ,IEcsRunSystem
    {
        private readonly IUserBalanceService _userBalance;
        
        private EcsFilter _filter;
        private EcsWorld _world;

        public RefreshUserBalanceSystem(IUserBalanceService userBalanceService)
        {
            _userBalance = userBalanceService;
        }
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<UserBalanceComponent>().End();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int balanceEntity in _filter)
            {   
                ref UserBalanceComponent balance = ref _world
                    .GetPool<UserBalanceComponent>()
                    .Get(balanceEntity);
                
                _userBalance.SetCurrentBalance(balance.Coins);
            }
        }
    }
}