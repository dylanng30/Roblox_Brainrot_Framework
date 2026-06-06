using System;

namespace Dylanng.Core.Data
{
    [Serializable]
    public class SaveData
    {
        public float MasterVolume = 1.0f;
        public int Coins = 0;
        public int Gems = 0;
    }
    
    [Serializable]
    public class AudioSaveData
    {
        // Music
        public bool IsMusicOn = true;
        public float MasterVolume = 1.0f;
        
        // Sound
        public bool IsSoundOn = true;
        public float MusicVolume = 1.0f;
    }
}
