using Code.Gameplay.Business.Components;
using Code.Gameplay.BusinessUpgrades.Components;
using Code.Gameplay.Income.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Income.Systems
{
    public class CalculateIncomeModifiersSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _businesses;
        private EcsFilter _modifiers;

        public void Init(IEcsSystems systems)
        {
            _businesses = systems.GetWorld().Filter<BusinessComponent>()
                .Inc<ComposedIncomeModifier>()
                .End();

            _modifiers = systems.GetWorld()
                .Filter<BusinessUpgradeComponent>()
                .Inc<IncomeModifierComponent>()
                .Inc<PurchasedComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int businessEntity in _businesses)
            {
                var business = _businesses.GetWorld().GetPool<BusinessComponent>().Get(businessEntity);
                ref var composedModifier = ref _businesses.GetWorld().GetPool<ComposedIncomeModifier>().Get(businessEntity);
                
                int result = 0;
                foreach (int modifierEntity in _modifiers)
                {
                    var upgrade = _modifiers.GetWorld().GetPool<BusinessUpgradeComponent>().Get(modifierEntity);
                    var modifier = _modifiers.GetWorld().GetPool<IncomeModifierComponent>().Get(modifierEntity);
                    
                    if (business.BusinessId == upgrade.BusinessId)
                    {
                        result += modifier.Percent;
                    }
                }

                composedModifier.Percent = result;
            }
        }
    }
}