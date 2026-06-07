using System;
using UnityEngine;

namespace RobloxFW.MovementSystem.Utilities
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {
        public Action OnAnimEnter;
        public Action OnAnimTransition;
        public Action OnAnimExit;

        public void OnAnimationEnter()
        {
            OnAnimEnter?.Invoke();
        }

        public void OnAnimationTransition()
        {
            OnAnimTransition?.Invoke();
        }

        public void OnAnimationExit()
        {
            OnAnimExit?.Invoke();
        }
    }

}