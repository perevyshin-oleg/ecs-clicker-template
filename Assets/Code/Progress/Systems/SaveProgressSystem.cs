using System.Collections.Generic;
using Code.Gameplay.Balance.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.BusinessUpgrades.Components;
using Code.Gameplay.Income.Components;
using Code.Gameplay.LevelUp.Components;
using Code.Infrastructure.SavedLoadServices;
using Code.Progress.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Progress.Systems
{
    public class SaveProgressSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private readonly ISavedLoadService _savedLoadService;
        private EcsFilter _requests;

        public SaveProgressSystem(ISavedLoadService savedLoadService)
        {
            _savedLoadService = savedLoadService;
        }

        public void Init(IEcsSystems systems)
        {
            _requests = systems.GetWorld().Filter<SaveProgressRequest>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int request in _requests)
            {
                Save(systems);
                systems.GetWorld().GetPool<SaveProgressRequest>().Del(request);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            Save(systems);
        }

        private void Save(IEcsSystems systems)
        {
            SavedData savedData = new SavedData();
            SaveUpgrades(systems, savedData);
            SaveBalances(systems, savedData);
            SaveBusinesses(systems, savedData);
            _savedLoadService.Save(savedData);
        }
        
        private static void SaveBalances(IEcsSystems systems, SavedData savedData)
        {
            var userBalances = systems.GetWorld().Filter<UserBalanceComponent>()
                .End();

            foreach (int entity in userBalances)
            {
                var balance = systems.GetWorld().GetPool<UserBalanceComponent>().Get(entity);
                savedData.coins = balance.Coins;
            }
        }

        private static void SaveBusinesses(IEcsSystems systems, SavedData savedData)
        {
            var businesses = systems.GetWorld().Filter<BusinessComponent>()
                .Inc<LevelComponent>()
                .Inc<IncomeProgressComponent>()
                .End();

            foreach (int entity in businesses)
            {
                ref var levelComponent = ref systems.GetWorld().GetPool<LevelComponent>().Get(entity);
                var businessComponent = systems.GetWorld().GetPool<BusinessComponent>().Get(entity);
                var incomeProgress = systems.GetWorld().GetPool<IncomeProgressComponent>().Get(entity);
                
                SavedData.BusinessData businessData = new SavedData.BusinessData();
                businessData.businessId = businessComponent.BusinessId;
                businessData.level = levelComponent.Value;
                businessData.progress = incomeProgress.Progress;
                savedData.businessesData.Add(businessData);
            }
        }

        private static void SaveUpgrades(IEcsSystems systems, SavedData savedData)
        {
            var upgrades = systems.GetWorld().Filter<BusinessUpgradeComponent>()
                .End();

            foreach (int entity in upgrades)
            {
                var upgrade = systems.GetWorld().GetPool<BusinessUpgradeComponent>().Get(entity);
                bool isPurchased = systems.GetWorld().GetPool<PurchasedComponent>().Has(entity);
                
                SavedData.UpgradeData upgradeData = new SavedData.UpgradeData();
                upgradeData.upgradeId = upgrade.UpgradeId;
                upgradeData.isPurchased = isPurchased;
                savedData.upgradesData.Add(upgradeData);
            }
        }
    }
}