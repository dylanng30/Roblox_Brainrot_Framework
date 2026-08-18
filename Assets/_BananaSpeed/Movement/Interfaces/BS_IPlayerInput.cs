using System;
using UnityEngine;

namespace _BananaSpeed.Movement.States
{
    public interface BS_IPlayerInput
    {
        Vector2 MovementVector { get; }
        Vector2 RotationVector { get; }
        bool IsJumpHeld { get; }

        event Action OnJumpPressed;
    }
}