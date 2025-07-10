using UnityEngine;

namespace Code.Infrastructure.SavedLoadServices
{
    public class SavedLoadService : ISavedLoadService
    {
        private const string SavedDataPrefsPath = "SavedData";

        public bool HasSavedData {get; private set;} 
        
        public SavedData SavedData { get; private set; }

        public SavedLoadService()
        {
            Load();
        }
        
        public void Save(SavedData savedData)
        {
            SavedData = savedData;
            string json = JsonUtility.ToJson(savedData);
            PlayerPrefs.SetString(SavedDataPrefsPath, json);
            PlayerPrefs.Save();
        }

        private void Load()
        {
            string json = PlayerPrefs.GetString(SavedDataPrefsPath, null);
            
            if (string.IsNullOrEmpty(json))
            {
                HasSavedData = false;
                return;
            }
            
            SavedData = JsonUtility.FromJson<SavedData>(json);
            HasSavedData = true;
        }
    }
}