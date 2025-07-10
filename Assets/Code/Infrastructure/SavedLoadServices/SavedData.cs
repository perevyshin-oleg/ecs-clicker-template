using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Code.Infrastructure.SavedLoadServices
{
    [Serializable]
    public class SavedData
    {
        [Serializable]
        public class UpgradeData
        {
            public int upgradeId;
            public bool isPurchased;
        }

        [Serializable]
        public class BusinessData
        {
            public int businessId;
            public int level;
            public float progress;
        }
        
        public int coins;
        public List<UpgradeData> upgradesData = new List<UpgradeData>();
        public List<BusinessData> businessesData = new List<BusinessData>();
    }
}