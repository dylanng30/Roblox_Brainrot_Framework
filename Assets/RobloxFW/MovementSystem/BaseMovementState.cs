using Dylanng;

namespace RobloxFW.MovementSystem
{
    public abstract class BaseMovementState : IMovementState
    {
        public virtual void Enter()
        {
            GameLogger.Log($"{GetType().Name} Enter");
        }
        public virtual void HandleInput() {}
        public virtual void Update() {}
        public virtual void PhysicsUpdate() {}
        public virtual void Exit() {}

        public virtual void OnAnimationEnterEvent() {}

        public virtual void OnAnimationExitEvent()
        {
            GameLogger.Log($"{GetType().Name} Animation Exit");
        }
        public virtual void OnAnimationTransitionEvent() {}

        protected virtual void StartAnimation(int animationHash){}
        protected virtual void StopAnimation(int animationHash){}
    }
}