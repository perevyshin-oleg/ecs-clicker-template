using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Business.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/BusinessConfig", fileName = "BusinessConfig")]
    public class BusinessStaticData : ScriptableObject
    {
        [Serializable]
        public struct BusinessUpgradeData
        {
            [field: SerializeField] public string Name { get; private set; }
            [field: SerializeField] public int BaseCost { get; private set; }
            [field: SerializeField] public int IncomeModificator { get; private set; }
        }
        
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int InitialLevel { get; private set; }
        [field: SerializeField] public int BaseCost { get; private set; }
        [field: SerializeField] public int BaseIncome { get; private set; }
        [field: SerializeField] public float DurationInSeconds { get; private set; }
        [field: SerializeField] public List<BusinessUpgradeData> Upgrades { get; private set; }
    }
}