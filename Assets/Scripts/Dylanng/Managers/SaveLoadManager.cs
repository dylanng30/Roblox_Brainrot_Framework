using System.IO;
using Dylanng.Core;
using Dylanng.Core.Base;
using Dylanng.Core.Data;
using UnityEngine;

namespace Dylanng.Managers
{
    public class SaveLoadManager : ManagerBase
    {
        private string _savePath;
        public SaveData CurrentData { get; private set; }

        public override void Initialize()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "gamesave.json");
            ServiceLocator.Register<SaveLoadManager>(this);
            Load();
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(CurrentData, true);
            File.WriteAllText(_savePath, json);
            GameLogger.Log("Game Saved Successfully.");
        }

        public void Load()
        {
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);
                CurrentData = JsonUtility.FromJson<SaveData>(json);
                GameLogger.Log("Game Loaded Successfully.");
            }
            else
            {
                CurrentData = new SaveData(); // Default data
                Save();
            }
        }
    }
}
