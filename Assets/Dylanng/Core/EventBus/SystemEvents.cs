namespace Dylanng
{
    public interface IGameBootedEvent : IEvent {}
    
    public struct ExampleGameBootedEvent : IGameBootedEvent
    {
        
    }

    public struct GamePausedEvent : IEvent
    {
        public bool IsPaused;

        public GamePausedEvent(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }

    public struct GameStateChangedEvent : IEvent
    {
        public IGameState PreviousState;
        public IGameState NewState;

        public GameStateChangedEvent(IGameState previousState, IGameState newState)
        {
            PreviousState = previousState;
            NewState = newState;
        }
    }
}