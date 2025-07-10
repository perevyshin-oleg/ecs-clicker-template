namespace Code.Gameplay.BusinessUpgrades.UI
{
    public class UpgradeModel
    {
        public int Id { get; private set; }
        public int BusinessId {get; private set;}    
        public string Name { get; private set; }
        public int IncomeModifier { get; private set; }
        public int Cost { get; private set; }
        public bool IsPurchased { get; private set; }

        public UpgradeModel(int id, int businessId, string name, int incomeModifier, int cost, bool isPurchased)
        {
            Id = id;
            BusinessId = businessId;
            Name = name;
            IncomeModifier = incomeModifier;
            Cost = cost;
            IsPurchased = isPurchased;
        }

        public void SetPurchased() =>
            IsPurchased = true;
    }
}