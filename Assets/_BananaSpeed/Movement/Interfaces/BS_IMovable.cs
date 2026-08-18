using UnityEngine;

namespace _BananaSpeed.Movement.States
{
    public interface BS_IMovable
    {
        void SetHorizontalVelocity(Vector3 horizontalVelocity);
        void SetVerticalVelocity(float yVelocity);
        float GetVerticalVelocity();
        float GetCurrentYAngle();
        void AddImpulse(Vector3 force);
        void AddAcceleration(Vector3 force);
        void SetRotation(float yAngle);
    }
}