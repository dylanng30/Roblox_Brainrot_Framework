using Dylanng.Core.State;

namespace Dylanng.Core.State
{
    public abstract class HierarchicalStateBase : StateBase
    {
        protected StateMachine subStateMachine;

        public HierarchicalStateBase()
        {
            subStateMachine = new StateMachine();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            subStateMachine.Update();
            
            OnUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }

        protected abstract void OnUpdate();

        public void TransitionSubState(StateBase nextSubState)
        {
            subStateMachine.TransitionTo(nextSubState);
        }
    }
}
