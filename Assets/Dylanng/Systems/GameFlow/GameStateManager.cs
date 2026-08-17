using System;
using UnityEngine;

namespace Dylanng
{
    public class GameStateManager : ManagerBase, IGameStateService
    {
        public IGameState CurrentState { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            ServiceLocator.Register<IGameStateService>(this);
            GameLogger.Log("GameStateManager Initialized");
        }

        protected override void OnDestroy()
        {
            ServiceLocator.Unregister<IGameStateService>();
            base.OnDestroy();
        }

        private void Update()
        {
            CurrentState?.Update();
        }

        public void ChangeState<TState>() where TState : class, IGameState, new()
        {
            ChangeState(new TState());
        }

        public void ChangeState(IGameState newState)
        {
            if (CurrentState != null && CurrentState.GetType() == newState.GetType()) return;

            IGameState previousState = CurrentState;
            
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();

            GameLogger.Log($"Game State Changed: {previousState.GetType().Name} -> {newState.GetType().Name}");

            EventBus.Publish(new GameStateChangedEvent(previousState, CurrentState));
        }
    }
}
