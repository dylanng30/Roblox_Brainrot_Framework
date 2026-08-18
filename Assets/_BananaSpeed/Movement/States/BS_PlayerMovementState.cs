using UnityEngine;
using RobloxFW.MovementSystem;

namespace _BananaSpeed.Movement.States
{
    public class BS_PlayerMovementState : BaseMovementState
    {
        protected BS_PlayerMovementStateMachine _stateMachine;

        public BS_PlayerMovementState(BS_PlayerMovementStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log($"[BS_DEBUG] Entered State: {this.GetType().Name}");
        }

        protected Vector3 GetWorldMoveDirection()
        {
            var input = _stateMachine.Input.MovementVector;
            if (input == Vector2.zero) return Vector3.zero;

            Quaternion cameraYaw = Quaternion.Euler(
                0f, _stateMachine.CameraTransform.eulerAngles.y, 0f);
            return cameraYaw * new Vector3(input.x, 0f, input.y);
        }

        protected void UpdateSpeed(float targetSpeed)
        {
            var data = _stateMachine.Data.Ground;
            float smoothTime = (targetSpeed > _stateMachine.SharedData.CurrentSpeed)
                ? data.AccelerationTime
                : data.DecelerationTime;

            _stateMachine.SharedData.CurrentSpeed = Mathf.SmoothDamp(
                _stateMachine.SharedData.CurrentSpeed,
                targetSpeed,
                ref _stateMachine.SharedData.SpeedSmoothVelocity,
                smoothTime);

            float normalizedSpeed = (data.MaxSpeed > 0f)
                ? _stateMachine.SharedData.CurrentSpeed / data.MaxSpeed
                : 0f;

            _stateMachine.Animation.SetAnimationFloat(_stateMachine.Data.AnimationData.SpeedHash, normalizedSpeed);
        }

        protected void ApplyHorizontalVelocity(Vector3 worldDir)
        {
            float speed = _stateMachine.SharedData.CurrentSpeed;
            _stateMachine.Movement.SetHorizontalVelocity(worldDir * speed);
        }

        protected void RotateTowardsMoveDirection(Vector3 worldDir)
        {
            if (worldDir == Vector3.zero) return;
            float targetAngle = Mathf.Atan2(worldDir.x, worldDir.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(
                _stateMachine.Movement.GetCurrentYAngle(),
                targetAngle,
                ref _stateMachine.SharedData.RotationSmoothVelocity,
                _stateMachine.Data.Ground.RotationSmoothTime);
            _stateMachine.Movement.SetRotation(smoothAngle);
        }

        protected void ApplyFallGravity()
        {
            if (_stateMachine.Movement.GetVerticalVelocity() < 0f)
            {
                _stateMachine.Movement.AddAcceleration(
                    Physics.gravity * (_stateMachine.Data.Airborne.FallGravityMultiplier - 1f));
            }
        }

        protected void ResetVerticalVelocity()
        {
            _stateMachine.Movement.SetVerticalVelocity(0f);
        }
    }
}

