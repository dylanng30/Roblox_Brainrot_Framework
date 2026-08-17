using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dylanng
{
    public class SaveLoadManager : ManagerBase, ISaveLoadManager
    {
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public override void Initialize()
        {
            base.Initialize();

            ServiceLocator.Register<ISaveLoadManager>(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ServiceLocator.Unregister<ISaveLoadManager>();
        }

        public void RegisterRepository<T>(ISaveLoadService<T> repository)
        {
            var type = typeof(T);
            if (_repositories.ContainsKey(type) &&
                _repositories[type] != repository)
            {
                GameLogger.LogWarning("[SaveLoadManager] Repository for type " + type.Name + " is already registered. Ignoring registration.");
                _repositories[type] = repository;
                return;
            }

            _repositories[type] = repository;
        }

        public void UnregisterRepository<T>()
        {
            var type = typeof(T);
            if (_repositories.ContainsKey(type)) _repositories.Remove(type);
        }

        private ISaveLoadService<T> GetRepository<T>()
        {
            var type = typeof(T);
            if (_repositories.TryGetValue(type, out var repoObj))
            {
                return repoObj as ISaveLoadService<T>;
            }

            var defaultRepo = new PlayerPrefRepository<T>();
            _repositories[type] = defaultRepo;
            return defaultRepo;
        }

        public void Save<T>(T data, string keyName)
        {
            var repo = GetRepository<T>();
            repo.Save(data, keyName);
        }

        public T Load<T>(string keyName)
        {
            var repo = GetRepository<T>();
            return repo.Load(keyName);
        }

        public bool Exist(string keyName)
        {
            return PlayerPrefs.HasKey(keyName);
        }

        public bool Exist<T>(string keyName)
        {
            var repo = GetRepository<T>();
            return repo.Exist(keyName);
        }

        public void Delete(string keyName)
        {
            if (PlayerPrefs.HasKey(keyName))
            {
                PlayerPrefs.DeleteKey(keyName);
                PlayerPrefs.Save();
            }
        }
    }
}
