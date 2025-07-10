using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Business.StaticData;
using Code.Infrastructure.StaticDataProviders;
using UnityEngine;

public class StaticDataProvider : IStaticDataProvider
{
    private const string GameStaticDataPath = "StaticData/GameStaticData";
    private const string NamesStaticDataPath = "StaticData/NamesStaticData";

    public GameStaticData GameStaticData { get; private set; }
    public NamesStaticData NamesStaticData { get; private set; }

    public StaticDataProvider()
    {
        GameStaticData = Resources.Load<GameStaticData>(GameStaticDataPath);
        NamesStaticData = Resources.Load<NamesStaticData>(NamesStaticDataPath);
    }

    public string GetNameByKey(string key)
    {
        NamesStaticData.KeyNamePair pair = NamesStaticData.Names.FirstOrDefault(pair => pair.Key == key);
        
        if (pair == null)
        {
            Debug.LogError($"Key {key} not found in NamesStaticData");
            return string.Empty;
        }

        return pair.Name;
    }
}