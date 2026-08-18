using System;
using System.Collections;
using Dylanng;
using UnityEngine;

namespace _BananaSpeed.Bootstrapper
{
    public class BS_GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private ManagerBase[] managers;

        private void Awake()
        {
            StartCoroutine(InitializeFramework());
        }

        private IEnumerator InitializeFramework()
        {
            PauseSystem pauseSystem = new PauseSystem();
            pauseSystem.Initialize();
            yield return null;
 
            TickDriver driver = new GameObject("[TickDriver]").AddComponent<TickDriver>();
            TickSystem tickSystem = new TickSystem();
            tickSystem.Initialize();
            driver.Initialize(tickSystem);            
            yield return null;
            
            foreach (var manager in managers)
            {
                manager.Initialize();
                yield return null;
            }
            
            yield return null;
            EventBus.Publish<IGameBootedEvent>(new ExampleGameBootedEvent());
        }

        private void OnDestroy()
        {
            ServiceLocator.ClearAll();
            EventBus.ClearAll();
        }
    }
}