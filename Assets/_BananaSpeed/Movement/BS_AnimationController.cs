using System;
using _BananaSpeed.Movement.Interfaces;
using UnityEngine;

namespace _BananaSpeed.Movement
{
    [RequireComponent(typeof(Animator))]
    public class BS_AnimationController : MonoBehaviour, BS_IAnimationController
    {
        [SerializeField] private Animator animator;

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        public void StartAnimation(int animationHash)
        {
            animator.SetTrigger(animationHash);
        }

        public void StopAnimation(int animationHash)
        {
            animator.ResetTrigger(animationHash);
        }

        public void SetAnimationFloat(int animationHash, float value)
        {
            animator.SetFloat(animationHash, value);
        }

        public void SetAnimationBool(int animationHash, bool value)
        {
            animator.SetBool(animationHash, value);
        }

        public event Action OnAnimationEnterEvent;
        public event Action OnAnimationExitEvent;
        public event Action OnAnimationTransitionEvent;

        public void OnAnimationEnter()
        {
            OnAnimationEnterEvent?.Invoke();
        }

        public void OnAnimationExit()
        {
            OnAnimationExitEvent?.Invoke();
        }

        public void OnAnimationTransition()
        {
            OnAnimationTransitionEvent?.Invoke();
        }
    }
}
