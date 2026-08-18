namespace RobloxFW.MovementSystem
{
    public interface IMovementState
    {
        void Enter();
        void Exit();
        void HandleInput();
        void Update();
        void PhysicsUpdate();
    
        void OnAnimationEnterEvent();
        void OnAnimationExitEvent();
        void OnAnimationTransitionEvent();
    }
}