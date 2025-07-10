using Code.Gameplay.Balance.Components;
using Code.Gameplay.Balance.Services;
using Code.Gameplay.Business.Components;
using Code.Gameplay.LevelUp.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Gameplay.Balance.Systems
{
    public class ProcessLevelUpRequestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IUserBalanceService _userBalance;
        private EcsFilter _requests;
        private EcsFilter _balances;
        private EcsFilter _businesses;

        public ProcessLevelUpRequestSystem(IUserBalanceService userBalance)
        {
            _userBalance = userBalance;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _requests = world.Filter<LevelUpRequest>().End();
            _balances = world.Filter<UserBalanceComponent>().End();
            _businesses = world.Filter<BusinessComponent>()
                .Inc<TotalCostComponent>()
                .Inc<LevelComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int requestEntity in _requests)
            foreach (int balanceEntity in _balances)
            foreach (int businessEntity in _businesses)
            {
                LevelUpRequest request = _requests.GetWorld().GetPool<LevelUpRequest>().Get(requestEntity);
                BusinessComponent business = _businesses.GetWorld().GetPool<BusinessComponent>().Get(businessEntity);

                if (business.BusinessId == request.BusinessId)
                {
                    ref UserBalanceComponent balance =
                        ref _balances.GetWorld().GetPool<UserBalanceComponent>().Get(balanceEntity);

                    ref LevelComponent level =
                        ref _businesses.GetWorld().GetPool<LevelComponent>().Get(businessEntity);

                    TotalCostComponent totalCost =
                        _businesses.GetWorld().GetPool<TotalCostComponent>().Get(businessEntity);

                    if (_userBalance.TrySpendCoins(totalCost.Value))
                    {
                        balance.Coins = _userBalance.CurrentBalance;
                        level.Value = Mathf.Clamp(level.Value + 1, 0, int.MaxValue);
                    }
                }
            }
        }
    }
}