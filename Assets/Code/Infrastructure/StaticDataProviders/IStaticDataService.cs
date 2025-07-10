using Code.Gameplay.Business.StaticData;
using Code.Infrastructure.StaticDataProviders;

public interface IStaticDataProvider
{
    GameStaticData GameStaticData { get; }
    NamesStaticData NamesStaticData { get; }
    string GetNameByKey(string key);
}