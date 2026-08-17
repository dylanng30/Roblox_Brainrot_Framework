namespace Dylanng
{
    public interface IGameStateService : IService
    {
        IGameState CurrentState { get; }
        void ChangeState<TState>() where TState : class, IGameState, new();
        void ChangeState(IGameState newState);
    }
}
