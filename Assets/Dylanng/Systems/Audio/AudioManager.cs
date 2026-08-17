using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dylanng
{
    [Serializable]
    public struct SoundData
    {
        public SoundType Type;
        public AudioClip[] Clips;
        [Range(0f, 1f)] public float Volume;
        [Range(-3f, 3f)] public float Pitch;
        public float PitchRandomness;
    }

    public class AudioManager : ManagerBase
    {
        [Header("--- AUDIO SOURCES ---")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        
        [Header("--- SOUND LIBRARY ---")]
        [SerializeField] private SoundLibrarySO soundLibrary;
        [SerializeField] private SoundType bgMusic;

        private Dictionary<SoundType, SoundData> _sounds;
        
        public SoundSettingsData SettingsData { get; private set; }

        public override void Initialize()
        {
            _sounds = new Dictionary<SoundType, SoundData>();
            foreach (var sound in soundLibrary.Sounds)
            {
                _sounds[sound.Type] = sound;
            }

            ServiceLocator.Register(this);
            
            PlayMusic(bgMusic);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ServiceLocator.Unregister<AudioManager>();
        }

        public void PlayMusic(SoundType type, bool loop = true)
        {
            if (SettingsData != null) return;

            if (soundLibrary != null && _sounds.TryGetValue(type, out var soundData))
            {
                if (soundData.Clips == null || soundData.Clips.Length == 0 || musicSource.clip == soundData.Clips[0]) return;

                musicSource.clip = soundData.Clips[Random.Range(0, soundData.Clips.Length)];
                float pitch = soundData.Pitch == 0f ? 1f : soundData.Pitch + Random.Range(-soundData.PitchRandomness, soundData.PitchRandomness);
                float volume = soundData.Volume == 0f ? 1f : soundData.Volume;
                musicSource.volume = volume;
                musicSource.pitch = pitch;
                musicSource.loop = loop;
                musicSource.Play();
            }
            else
            {
                GameLogger.LogWarning($"AudioManager: SoundType {type} chưa được thiết lập trong thư viện!");
            }
        }

        public void PlaySFX(SoundType type)
        {
            if (SettingsData != null && !SettingsData.IsSfxOn) return;

            if (soundLibrary != null && _sounds.TryGetValue(type, out var soundData))
            {
                if (soundData.Clips == null || soundData.Clips.Length == 0) return;
                float pitch = soundData.Pitch == 0f ? 1f : soundData.Pitch + Random.Range(-soundData.PitchRandomness, soundData.PitchRandomness);
                sfxSource.pitch = pitch;
                float volume = soundData.Volume == 0f ? 1f : soundData.Volume;
                sfxSource.PlayOneShot(soundData.Clips[Random.Range(0, soundData.Clips.Length)], volume);
            }
            else
            {
                GameLogger.LogWarning($"AudioManager: SoundType {type} chưa được thiết lập trong thư viện");
            }
        }

        public void SetMusicMute(bool mute)
        {
            musicSource.mute = mute;
            SettingsData.IsMusicOn = !mute;
        }

        public void SetSfxMute(bool mute)
        {
            sfxSource.mute = mute;
            SettingsData.IsSfxOn = !mute;
        }

        public void SetMasterVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            musicSource.volume = volume;
            sfxSource.volume = volume;
        }
    }
}