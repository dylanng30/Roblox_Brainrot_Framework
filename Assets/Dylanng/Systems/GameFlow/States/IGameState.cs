using System;

namespace Dylanng
{
    public interface IGameState
    {
        void Enter();
        void Update();
        void Exit();
    }
}
