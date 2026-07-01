using System;
using System.Collections;
using Dylanng.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RobloxFW.GameModeSystem
{
    public class ModeTransitionManager : PersistentSingleton<ModeTransitionManager>
    {
        public event Action OnTransitionStarted;
        public event Action OnTransitionCompleted;
        
        public void SwitchToMode(string sceneName)
        {
            StartCoroutine(TransitionRoutine(sceneName));
        }
        private IEnumerator TransitionRoutine(string sceneName)
        {
            OnTransitionStarted?.Invoke();
            
            // ServiceLocator.Resolve<TickSystem>().Pause();
            
            // yield return SceneManager.LoadSceneAsync("LoadingScene");
            
            TeardownCurrentState();

            yield return Resources.UnloadUnusedAssets();
            GC.Collect();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncLoad.isDone)
            {

                yield return null;
            }

            OnTransitionCompleted?.Invoke();
        }
        private void TeardownCurrentState()
        {
            // ServiceLocator.Resolve<EventBus>().ClearAllListeners();
            // ServiceLocator.Resolve<PoolManager>().ClearAllPools();
            // ServiceLocator.Resolve<TickSystem>().ClearAllTickables();
        }
    }
}