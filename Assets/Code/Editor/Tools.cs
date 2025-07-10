#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class PlayerPrefsCleaner
{
    [MenuItem("Tools/Clear All PlayerPrefs")]
    private static void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("<color=green>Все PlayerPrefs очищены!</color>");
    }
}
#endif
