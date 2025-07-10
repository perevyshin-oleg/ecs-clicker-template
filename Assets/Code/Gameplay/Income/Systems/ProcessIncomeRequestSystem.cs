using Code.Gameplay.Balance.Components;
using Code.Gameplay.Income.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Gameplay.Income.Systems
{
    public class ProcessIncomeRequestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _requests;
        private EcsFilter _userBalances;

        public void Init(IEcsSystems systems)
        {
            _requests = systems.GetWorld().Filter<IncomeRequest>()
                .Inc<TotalIncomeComponent>()
                .End();

            _userBalances = systems.GetWorld().Filter<UserBalanceComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int request in _requests)
            foreach (var balanceEntity in _userBalances)
            {
                ref UserBalanceComponent balance = ref _userBalances
                    .GetWorld().GetPool<UserBalanceComponent>().Get(balanceEntity);

                TotalIncomeComponent income = _requests
                    .GetWorld().GetPool<TotalIncomeComponent>().Get(request);

                balance.Coins = Mathf.Clamp(balance.Coins + income.Value, 0, int.MaxValue);

                _requests.GetWorld().GetPool<IncomeRequest>().Del(request);
            }
        }
    }
}