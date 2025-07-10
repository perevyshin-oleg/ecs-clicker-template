using System.Linq;
using Code.Gameplay.Balance.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.BusinessUpgrades.Components;
using Code.Gameplay.Income.Components;
using Code.Gameplay.LevelUp.Components;
using Code.Infrastructure.SavedLoadServices;
using Leopotam.EcsLite;

namespace Code.Progress.Systems
{
    public class LoadProgressSystem : IEcsInitSystem
    {
        private readonly ISavedLoadService _savedLoadService;

        public LoadProgressSystem(ISavedLoadService savedLoadService)
        {
            _savedLoadService = savedLoadService;
        }

        public void Init(IEcsSystems systems)
        {
            if (!_savedLoadService.HasSavedData)
            {
                return;
            }

            SavedData savedData = _savedLoadService.SavedData;

            LoadUpgrades(systems, savedData);
            LoadBusinesses(systems, savedData);
            LoadBalances(systems, savedData);
        }

        private static void LoadBalances(IEcsSystems systems, SavedData savedData)
        {
            EcsFilter userBalances = systems.GetWorld().Filter<UserBalanceComponent>()
                .End();

            foreach (int entity in userBalances)
            {
                ref UserBalanceComponent balance = ref systems
                    .GetWorld().GetPool<UserBalanceComponent>().Get(entity);
                
                balance.Coins = savedData.coins;
            }
        }

        private static void LoadBusinesses(IEcsSystems systems, SavedData savedData)
        {
            EcsFilter businesses = systems.GetWorld().Filter<BusinessComponent>()
                .Inc<LevelComponent>()
                .Inc<IncomeProgressComponent>()
                .End();

            foreach (int entity in businesses)
            {
                BusinessComponent businessComponent = systems
                    .GetWorld().GetPool<BusinessComponent>().Get(entity);
                ref LevelComponent levelComponent = ref systems
                    .GetWorld().GetPool<LevelComponent>().Get(entity);
                ref IncomeProgressComponent incomeProgress = ref systems
                    .GetWorld().GetPool<IncomeProgressComponent>().Get(entity);

                SavedData.BusinessData businessData = savedData.businessesData
                    .FirstOrDefault(b => b.businessId == businessComponent.BusinessId);

                if (businessData != null)
                {
                    incomeProgress.Progress = businessData.progress;
                    levelComponent.Value = businessData.level;
                }
            }
        }

        private static void LoadUpgrades(IEcsSystems systems, SavedData savedData)
        {
            var upgrades = systems.GetWorld().Filter<BusinessUpgradeComponent>()
                .End();

            foreach (int entity in upgrades)
            {
                BusinessUpgradeComponent upgrade = systems
                    .GetWorld().GetPool<BusinessUpgradeComponent>().Get(entity);

                bool isPurchased = savedData.upgradesData
                    .Any(u => u.upgradeId == upgrade.UpgradeId && u.isPurchased);

                if (isPurchased)
                {
                    systems.GetWorld().GetPool<PurchasedComponent>().Add(entity);
                }
            }
        }
    }
}