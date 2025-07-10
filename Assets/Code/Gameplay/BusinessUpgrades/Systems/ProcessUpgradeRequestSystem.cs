using Code.Gameplay.Balance.Components;
using Code.Gameplay.Balance.Services;
using Code.Gameplay.Business.Components;
using Code.Gameplay.BusinessUpgrades.Components;
using Code.Gameplay.BusinessUpgrades.Services;
using Code.Gameplay.Income.Components;
using Code.Gameplay.LevelUp.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Gameplay.BusinessUpgrades.Systems
{
    public class ProcessUpgradeRequestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IUserBalanceService _userBalance;
        private readonly IUpgradesService _upgradeService;
        private EcsFilter _requests;
        private EcsFilter _balances;
        private EcsFilter _upgrades;

        public ProcessUpgradeRequestSystem(IUserBalanceService userBalance, IUpgradesService upgradeService)
        {
            _userBalance = userBalance;
            _upgradeService = upgradeService;
        }
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _requests = world.Filter<BusinessUpgradeRequest>().End();
            _balances = world.Filter<UserBalanceComponent>().End();
            _upgrades = world.Filter<BusinessUpgradeComponent>()
                .Inc<TotalCostComponent>()
                .Exc<PurchasedComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int requestEntity in _requests)
            foreach (int balanceEntity in _balances)
            foreach (int upgradeEntity in _upgrades)
            {
                BusinessUpgradeRequest request = _requests.GetWorld()
                    .GetPool<BusinessUpgradeRequest>().Get(requestEntity);
                BusinessUpgradeComponent upgrade = _upgrades.GetWorld()
                    .GetPool<BusinessUpgradeComponent>().Get(upgradeEntity);
                
                if (upgrade.UpgradeId == request.UpgradeId)
                {
                    ref UserBalanceComponent balance = ref _balances.GetWorld()
                        .GetPool<UserBalanceComponent>().Get(balanceEntity);

                    TotalCostComponent totalCost = _upgrades.GetWorld()
                        .GetPool<TotalCostComponent>().Get(upgradeEntity);

                    if (_userBalance.TrySpendCoins(totalCost.Value))
                    {
                        balance.Coins = _userBalance.CurrentBalance;
                        _upgrades.GetWorld().GetPool<PurchasedComponent>().Add(upgradeEntity);
                        _upgradeService.PurchaseUpgrade(upgrade.UpgradeId);
                    }
                }
            }
        }
    }
}
