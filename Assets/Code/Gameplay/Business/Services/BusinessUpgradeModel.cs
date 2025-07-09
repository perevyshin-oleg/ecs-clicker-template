namespace Code.Gameplay.Business.Services
{
    public class BusinessUpgradeModel
    {
        public string Name { get; private set; }
        public int Income { get; private set; }
        public int Cost { get; private set; }
        public bool IsPurchased { get; private set; }
    }
}