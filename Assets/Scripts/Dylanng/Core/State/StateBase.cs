namespace Dylanng.Core.State
{
    public abstract class StateBase : IMovementState
    {
        public virtual void Enter()
        {
            //Debug.Log($"{GetType().Name} Enter");
        }
        public virtual void HandleInput() {}
        public virtual void Update() {}
        public virtual void PhysicsUpdate() {}
        public virtual void Exit() {}
        
        public virtual void OnAnimationEnterEvent() {}

        public virtual void OnAnimationExitEvent()
        {
            //Debug.Log($"{GetType().Name} Animation Exit");
        }
        public virtual void OnAnimationTransitionEvent() {}
        
        protected virtual void StartAnimation(int animationHash){}
        protected virtual void StopAnimation(int animationHash){}
    }
}
