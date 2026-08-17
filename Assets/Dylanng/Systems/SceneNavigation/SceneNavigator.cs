using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dylanng
{
    public class SceneNavigator : PersistentSingleton<SceneNavigator>
    {
        [SerializeField] private BaseLoadingOverlay loadingOverlay;
        
        private Dictionary<string, string> loadedSceneBySlot = new Dictionary<string, string>();
        
        private bool _isBusy = false;

        protected override void Awake()
        {
            if (loadingOverlay == null)
            {
                loadingOverlay = GetComponentInChildren<BaseLoadingOverlay>(true);
            }
            
            if (loadingOverlay == null)
            {
                GameLogger.LogError($"[SceneNavigator] No loading overlay found");
                return;
            }

            loadingOverlay.SetActive(true);
        }
        
        // API
        public SceneTransitionPlan Perform(SceneNavigationSO navigationData)
        {
            var plan = new SceneTransitionPlan();
            
            if (navigationData.UnloadSceneSlots != null)
            {
                foreach (string unloadSceneSlot in navigationData.UnloadSceneSlots)
                {
                    plan.Unload(unloadSceneSlot);
                }
            }
            
            plan.Load(navigationData.ActiveScene.SlotKey, navigationData.ActiveScene.SceneName, setActive: true);
            
            if (navigationData.AdditiveScenes != null)
            {
                foreach (var additiveScene in navigationData.AdditiveScenes)
                {
                    plan.Load(additiveScene.SlotKey, additiveScene.SceneName, setActive: false); 
                }
            }
            
            if (navigationData.HasOverlay)
            {
                plan.WithOverlay();
            }

            if (navigationData.ClearUnusedAssets)
            {
                plan.WithClearUnusedAssets();
            }

            return plan;
        }
        
        // Implementations
        private Coroutine ExecutePlan(SceneTransitionPlan plan)
        {
            if (_isBusy)
            {
                Debug.LogWarning("Scene change already in progress");
                return null;
            }
            
            _isBusy = true;
            return StartCoroutine(ChangeSceneCoroutine(plan));
        }

        private IEnumerator ChangeSceneCoroutine(SceneTransitionPlan plan)
        {
            if (plan.Overlay)
            {
                yield return loadingOverlay.StartLoad();
                yield return new WaitForSeconds(0.5f);
            }

            foreach (var slotKey in plan.SceneToUnload)
            {
                yield return UnloadSceneCoroutine(slotKey);
            }

            if (plan.ClearUnusedAssets) yield return CleanupUnusedAssetsCoroutine();

            foreach (var kvp in plan.ScenesToLoad)
            {
                if (loadedSceneBySlot.ContainsKey(kvp.Key))
                {
                    yield return UnloadSceneCoroutine(kvp.Key);
                }

                yield return LoadAdditiveCoroutine(kvp.Key, kvp.Value, plan.ActiveSceneName == kvp.Value);
            }

            if (plan.Overlay)
            {
                yield return loadingOverlay.EndLoad();
            }
            
            _isBusy = false;
        }

        private IEnumerator LoadAdditiveCoroutine(string slotKey, string sceneName, bool setActive)
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (loadOp == null) yield break;
            
            loadOp.allowSceneActivation = false;
            while (loadOp.progress < 0.9f)
            {
                if (loadingOverlay is ProgressLoadingOverlay progressOverlay)
                {
                    progressOverlay.UpdateProgress(loadOp.progress / 0.9f); 
                }
                yield return null;
            }
            
            loadOp.allowSceneActivation = true;
            while (!loadOp.isDone)
            {
                yield return null;
            }

            if (setActive)
            {
                Scene newScene = SceneManager.GetSceneByName(sceneName);
                if (newScene.IsValid() && newScene.isLoaded)
                {
                    SceneManager.SetActiveScene(newScene);
                }
            }
            
            loadedSceneBySlot[slotKey] = sceneName;
        }

        private IEnumerator UnloadSceneCoroutine(string slotKey)
        {
            if (!loadedSceneBySlot.TryGetValue(slotKey, out string sceneName)) yield break;
            if (string.IsNullOrEmpty(sceneName)) yield break;
            
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
            if (unloadOp != null)
            {
                while (!unloadOp.isDone)
                {
                    yield return null;
                }
            }
            loadedSceneBySlot.Remove(slotKey);
        }

        private IEnumerator CleanupUnusedAssetsCoroutine()
        {
            AsyncOperation cleanupOp = Resources.UnloadUnusedAssets();
            while (!cleanupOp.isDone)
            {
                yield return null;
            }
        }

        // Transition
        public class SceneTransitionPlan
        {
            public Dictionary<string, string> ScenesToLoad = new Dictionary<string, string>();
            public List<string> SceneToUnload { get; } = new List<string>();
            public string ActiveSceneName { get; private set; }
            public bool ClearUnusedAssets { get; private set; }
            public bool Overlay { get; private set; } = false;

            public SceneTransitionPlan Load(string slotKey, string sceneName, bool setActive = false)
            {
                ScenesToLoad[slotKey] = sceneName;
                if (setActive) ActiveSceneName = sceneName;
                return this;
            }

            public SceneTransitionPlan Unload(string slotKey)
            {
                SceneToUnload.Add(slotKey);
                return this;
            }
            
            public SceneTransitionPlan WithOverlay()
            {
                Overlay = true;
                return this;
            }

            public SceneTransitionPlan WithClearUnusedAssets()
            {
                ClearUnusedAssets = true;
                return this;
            }

            public Coroutine Perform()
            {
                return SceneNavigator.Instance.ExecutePlan(this);
            }
        }
    }
}