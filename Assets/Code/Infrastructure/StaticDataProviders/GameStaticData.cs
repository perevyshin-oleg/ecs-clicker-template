using System.Collections.Generic;
using Code.Gameplay.Business.StaticData;
using UnityEngine;

namespace Code.Infrastructure.StaticDataProviders
{
    [CreateAssetMenu(menuName = "StaticData/GameStaticData", fileName = "GameStaticData")]
    public class GameStaticData : ScriptableObject
    {
        [field: SerializeField] public List<BusinessStaticData> Businesses { get; private set; }
    }
}
