using System.Collections.Generic;

namespace Dylanng.Core.State
{
    public class PushdownStateMachine
    {
        private Stack<StateBase> _stateStack = new Stack<StateBase>();

        public StateBase CurrentState => _stateStack.Count > 0 ? _stateStack.Peek() : null;

        public void PushState(StateBase newState)
        {
            CurrentState?.Exit();
            _stateStack.Push(newState);
            newState.Enter();
        }

        public void PopState()
        {
            if (_stateStack.Count > 0)
            {
                _stateStack.Peek().Exit();
                _stateStack.Pop();
                CurrentState?.Enter(); // Resume previous state
            }
        }

        public void ChangeState(StateBase newState)
        {
            if (_stateStack.Count > 0)
            {
                _stateStack.Peek().Exit();
                _stateStack.Pop();
            }
            _stateStack.Push(newState);
            newState.Enter();
        }

        public void Update()
        {
            CurrentState?.Update();
        }
    }
}
