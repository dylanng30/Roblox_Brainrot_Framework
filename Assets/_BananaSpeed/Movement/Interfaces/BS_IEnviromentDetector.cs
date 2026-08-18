namespace _BananaSpeed.Movement.States
{
    public interface BS_IEnviromentDetector
    {
        bool IsGrounded { get; }
        bool CanClimb { get; }
    }
}