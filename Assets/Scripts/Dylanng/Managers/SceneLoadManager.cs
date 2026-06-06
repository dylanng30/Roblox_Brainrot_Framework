using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Dylanng.Core;
using Dylanng.Core.Base;

namespace Dylanng.Managers
{
    public class SceneLoadManager : ManagerBase
    {
        public override void Initialize()
        {
            ServiceLocator.Register<SceneLoadManager>(this);
            GameLogger.Log("SceneLoadManager Initialized");
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            // TODO: show Loading Canvas

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }
            
            asyncLoad.allowSceneActivation = true;

            while (!asyncLoad.isDone)
            {
                yield return null;
            }


        }
    }
}