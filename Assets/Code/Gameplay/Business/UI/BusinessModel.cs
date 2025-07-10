using System;

namespace Code.Gameplay.Business.Services
{
    public class BusinessModel
    {
        public event Action Updated;
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public int TotalCost { get; private set; }
        public int TotalIncome { get; private set; }
        public float IncomeProgress { get; private set; }

        public BusinessModel(int id, string name, int level, int totalCost, int totalIncome)
        {
            Id = id;
            Name = name;
            Level = level;
            TotalCost = totalCost;
            TotalIncome = totalIncome;
        }

        public void SetLevel(int level)
        {
            if (Level != level)
            {
                Level = level;
                Updated?.Invoke();
            }
        }

        public void SetTotalCost(int totalCost)
        {
            if (TotalCost != totalCost)
            {
                TotalCost = totalCost;
                Updated?.Invoke();
            }
        }

        public void SetTotalIncome(int totalIncome)
        {
            if (TotalIncome != totalIncome)
            {
                TotalIncome = totalIncome;
                Updated?.Invoke();
            }
        }

        public void SetIncomeProgress(float progress)
        {
            if (IncomeProgress != progress)
            {
                IncomeProgress = progress;
                Updated?.Invoke();
            }
        }
    }
}