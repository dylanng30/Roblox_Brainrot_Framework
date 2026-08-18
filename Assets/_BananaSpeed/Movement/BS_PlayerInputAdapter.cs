using System;
using UnityEngine;
using RobloxFW.InputSystem;
using _BananaSpeed.Movement.States;
using Dylanng;

namespace _BananaSpeed.Movement
{
    public class BS_PlayerInputAdapter : MonoBehaviour, BS_IPlayerInput
    {
        private IPlayerInput _robloxInput;

        public Vector2 MovementVector => _robloxInput != null ? _robloxInput.Movement : Vector2.zero;
        
        public Vector2 RotationVector => _robloxInput != null ? _robloxInput.Look : Vector2.zero;
        
        public bool IsJumpHeld => _robloxInput != null && _robloxInput.IsPressingSpace;

        public event Action OnJumpPressed;

        private bool _wasJumpHeld = false;

        private void Update()
        {
            if (_robloxInput == null)
            {
                _robloxInput = ServiceLocator.Get<GameInputManager>();
            }

            if (_robloxInput == null) return;

            bool isJumpHeldNow = _robloxInput.IsPressingSpace;

            if (isJumpHeldNow && !_wasJumpHeld)
            {
                OnJumpPressed?.Invoke();
            }

            _wasJumpHeld = isJumpHeldNow;
        }
    }
}
