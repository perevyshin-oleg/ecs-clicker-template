using Code.Gameplay.Balance.Services;
using Code.Gameplay.Balance.Systems;
using Code.Gameplay.Business.Services;
using Code.Gameplay.Business.Systems;
using Code.Gameplay.BusinessUpgrades.Systems;
using Code.Gameplay.Income.Systems;
using Code.Gameplay.LevelUp.Systems;
using Code.Gameplay.UI;
using Code.Infrastructure.StaticDataProviders;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Infrastructure
{
    public class EcsStartup : MonoBehaviour
    {
        private const string GameStaticDataPath = "Configs/GameStaticData"; 
        
        private EcsWorld _world;
        private IEcsSystems _systems;

        private IBusinessesService _businesses;
        private IUserBalanceService _userBalance;
        private BusinessElementFactory _businessElementFactory;
        
        private GameStaticData _gameData;

        void Start()
        {
            Initialize();
            
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            AddServices();
            AddFactories();
            AddSystems();
            _systems.Init();
        }

        private void Initialize()
        {
            _gameData = Resources.Load<GameStaticData>(GameStaticDataPath);
        }
        
        private void AddServices()
        {
            _userBalance = new UserBalanceService();
            _businesses = new BusinessesService(_world);
        }

        private void AddFactories()
        {
            _businessElementFactory = new BusinessElementFactory(_businesses);
        }

        private void AddSystems()
        {
            _systems
                .Add(new InitializeUserBalanceSystem())
                .Add(new InitializeBusinessesSystem(_gameData))
                .Add(new InitializeBusinessesUISystem(_businesses, _userBalance, _businessElementFactory))
                .Add(new MarkBusinessPurchasedSystem())
                .Add(new IncreaseIncomeProgressSystem())
                .Add(new CalculateTotalIncomeSystem())
                .Add(new ProcessIncomeRequestSystem())
                .Add(new ProcessLevelUpRequestSystem(_userBalance))
                .Add(new FinalizeLevelUpRequestSystem())
                .Add(new CalculateUpgradeCostSystem())
                //.Add(new ProcessUpgradeRequestSystem(_userBalance))
                .Add(new RefreshBusinessUISystem(_businesses))
                .Add(new RefreshUserBalanceSystem(_userBalance));
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
