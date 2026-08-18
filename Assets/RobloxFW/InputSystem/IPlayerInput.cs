using UnityEngine;

namespace RobloxFW.InputSystem
{
    public interface IPlayerInput
    {
        Vector2 Movement { get; }
        Vector2 Look { get; }
        bool IsPressingSpace { get; }
        bool IsPressingLeftControl { get; }
        bool IsPressingRightControl { get; }
        bool Dash { get; }
        bool Sprint { get; }
        
        bool PressLeftMouse();
        bool IsDraggingLeftMouse();
        bool DropLeftMouse();
    }
}
