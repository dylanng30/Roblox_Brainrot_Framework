namespace RobloxFW.MovementSystem
{
    public abstract class BaseMovementStateMachine
    {
        protected BaseMovementState CurrentMovementState;

        public void Initialize(BaseMovementState startingMovementState)
        {
            CurrentMovementState = startingMovementState;
            CurrentMovementState?.Enter();
        }

        public void TransitionTo(BaseMovementState nextState)
        {
            if (CurrentMovementState == nextState) return;

            CurrentMovementState?.Exit();
            CurrentMovementState = nextState;
            CurrentMovementState?.Enter();
        }

        public void Update()
        {
            CurrentMovementState?.Update();
        }

        public void PhysicsUpdate()
        {
            CurrentMovementState?.PhysicsUpdate();
        }

        public void HandleInput()
        {
            CurrentMovementState?.HandleInput();
        }

        public void OnAnimationEnterEvent()
        {
            CurrentMovementState?.OnAnimationEnterEvent();
        }

        public void OnAnimationExitEvent()
        {
            CurrentMovementState?.OnAnimationExitEvent();
        }

        public void OnAnimationTransitionEvent()
        {
            CurrentMovementState?.OnAnimationTransitionEvent();
        }

        public virtual void CleanUp()
        {
    
        }
    }
}