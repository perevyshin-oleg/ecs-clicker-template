using Code.Gameplay.Balance.Services;
using Code.Gameplay.Balance.Systems;
using Code.Gameplay.Business.Services;
using Code.Gameplay.Business.Systems;
using Code.Gameplay.BusinessUpgrades.Services;
using Code.Gameplay.BusinessUpgrades.Systems;
using Code.Gameplay.Income.Systems;
using Code.Gameplay.LevelUp.Systems;
using Code.Gameplay.UI;
using Code.Infrastructure.SavedLoadServices;
using Code.Infrastructure.StaticDataProviders;
using Code.Progress.Systems;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Infrastructure
{
    public class EcsStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private IEcsSystems _systems;
        private IBusinessesService _businesses;
        private IUserBalanceService _userBalance;
        private IUpgradesService _upgrades;
        private ISavedLoadService _savedLoadService;
        private BusinessElementFactory _businessElementFactory;
        private IStaticDataProvider _staticDataProvider;
        
        private GameStaticData _gameData;

        void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            AddServices();
            AddFactories();
            AddSystems();
            _systems.Init();
        }
        
        private void AddServices()
        {
            _savedLoadService = new SavedLoadService();
            _userBalance = new UserBalanceService();
            _businesses = new BusinessesService(_world);
            _upgrades = new UpgradesService(_world);
            _staticDataProvider = new StaticDataProvider();
        }

        private void AddFactories()
        {
            _businessElementFactory = new BusinessElementFactory(_businesses, _upgrades);
        }

        private void AddSystems()
        {
            _systems
                .Add(new InitializeUserBalanceSystem())
                .Add(new InitializeBusinessesSystem(_staticDataProvider))
                .Add(new InitializeUpgradesSystem(_staticDataProvider))
                .Add(new LoadProgressSystem(_savedLoadService))
                .Add(new InitializeBusinessesUISystem(_businesses, _userBalance, _businessElementFactory, _upgrades))
                .Add(new MarkBusinessPurchasedSystem())
                .Add(new IncreaseIncomeProgressSystem())
                .Add(new CalculateIncomeModifiersSystem())
                .Add(new CalculateTotalIncomeSystem())
                .Add(new ProcessIncomeRequestSystem())
                .Add(new ProcessLevelUpRequestSystem(_userBalance))
                .Add(new FinalizeLevelUpRequestSystem())
                .Add(new ProcessUpgradeRequestSystem(_userBalance, _upgrades))
                .Add(new FinalizeUpgradeRequestSystem())
                .Add(new CalculateUpgradeCostSystem())
                .Add(new RefreshBusinessUISystem(_businesses))
                .Add(new RefreshUserBalanceSystem(_userBalance))
                .Add(new SaveProgressSystem(_savedLoadService));
        }

        void Update()
        {
            _systems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }
        
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}
