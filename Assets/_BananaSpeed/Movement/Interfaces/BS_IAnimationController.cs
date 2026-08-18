using System;

namespace _BananaSpeed.Movement.Interfaces
{
    public interface BS_IAnimationController
    {
        void StartAnimation(int animationHash);
        void StopAnimation(int animationHash);
        void SetAnimationFloat(int animationHash, float value);
        void SetAnimationBool(int animationHash, bool value);

        event Action OnAnimationEnterEvent;
        event Action OnAnimationExitEvent;
        event Action OnAnimationTransitionEvent;
    }
}
