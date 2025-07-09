using System;

namespace Code.Gameplay.Balance.Services
{
    public class UserBalanceService : IUserBalanceService
    {
        public int CurrentBalance { get; private set; }
        
        public event Action<int> OnBalanceChanged;

        public void SetCurrentBalance(int newBalance)
        {
            if (CurrentBalance != newBalance)
            {
                CurrentBalance = newBalance;
                OnBalanceChanged?.Invoke(CurrentBalance);
            }
        }

        public bool TrySpendCoins(int amount)
        {
            if (CurrentBalance - amount < 0)
            {
                return false;
            }
            
            SetCurrentBalance(CurrentBalance - amount);
            return true;
        }
    }
}