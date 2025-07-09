using System.Collections.Generic;

namespace Code.Gameplay.BusinessUpgrades.Components
{
    public struct BusinessUpgradesComponent
    {
        public struct BusinessUpgrade
        {
            public int Id;
            public string Name;
            public int Cost;
            public bool IsPurchased;
            public int IncomeModificator;
        }
        
        public List<BusinessUpgrade> Upgrades;
    }
}