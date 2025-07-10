using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Business.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/NamesConfig", fileName = "NamesConfig")]
    public class NamesStaticData : ScriptableObject
    {
        [Serializable]
        public class KeyNamePair
        {
            [field: SerializeField] public string Key { get; private set; }
            [field: SerializeField] public string Name {get; private set;}
        }

        [field: SerializeField] public List<KeyNamePair> Names { get; private set; }
    }
}