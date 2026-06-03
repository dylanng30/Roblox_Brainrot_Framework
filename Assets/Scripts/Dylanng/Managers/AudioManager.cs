using UnityEngine;
using Dylanng.Core;
using Dylanng.Core.Base;
using Dylanng.Core.Data;

namespace Dylanng.Managers
{
    public class AudioManager : ManagerBase
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        public override void Initialize()
        {
            ServiceLocator.Register<AudioManager>(this);

            var saveData = ServiceLocator.Get<SaveLoadManager>()?.CurrentData;
            if (saveData != null)
            {
                SetMasterVolume(saveData.MasterVolume);
            }
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (clip == null || _musicSource.clip == clip) return;

            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;
            _sfxSource.PlayOneShot(clip);
        }

        public void SetMasterVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            _musicSource.volume = volume;
            _sfxSource.volume = volume;
        }
    }
}