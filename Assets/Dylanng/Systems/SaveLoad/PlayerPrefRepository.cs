using System;
using UnityEngine;

namespace Dylanng
{
    public class PlayerPrefRepository<T> : ISaveLoadService<T>
    {
        public bool Exist(string keyName)
        {
            return PlayerPrefs.HasKey(keyName);
        }       

        public T Load(string keyName)
        {
            if (!Exist(keyName))
            {
                return default;
            }

            Type type = typeof(T);
            switch (type)
            {
                case Type _ when type == typeof(int):
                    int value = PlayerPrefs.GetInt(keyName);
                    return (T)(object)value;
                case Type _ when type == typeof(float):
                    return (T)(object)PlayerPrefs.GetFloat(keyName);
                case Type _ when type == typeof(string):
                    return (T)(object)PlayerPrefs.GetString(keyName);
            }

            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(keyName));
        }

        public void Save(T data, string keyName)
        {
            switch (data)
            {
                case int intValue:
                    PlayerPrefs.SetInt(keyName, intValue);
                    break;
                case float floatValue:
                    PlayerPrefs.SetFloat(keyName, floatValue);
                    break;
                case string stringValue:
                    PlayerPrefs.SetString(keyName, stringValue);
                    break;
                default:
                    string json = JsonUtility.ToJson(data);
                    PlayerPrefs.SetString(keyName, json);
                    break;
            }

            PlayerPrefs.Save();
        }
    }
}