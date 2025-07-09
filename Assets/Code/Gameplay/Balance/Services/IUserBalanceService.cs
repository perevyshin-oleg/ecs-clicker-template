using System;

namespace Code.Gameplay.Balance.Services
{
    public interface IUserBalanceService
    {
        int CurrentBalance { get; }
        void SetCurrentBalance(int amount);
        bool TrySpendCoins(int amount);
        event Action<int> OnBalanceChanged;
    }
}