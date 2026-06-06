namespace Dylanng.Core.Systems.TickSystem
{
    public interface IUpdatable
    {
        void OnUpdate(float deltaTime);
    }

    public interface IFixedUpdatable
    {
        void OnFixedUpdate(float fixedDeltaTime);
    }

    public interface ILateUpdatable
    {
        void OnLateUpdate(float deltaTime);
    }

    public interface IOneSecondTickable
    {
        void OnOneSecondTick();
    }

    public interface ITickSystem : ISystem
    {
        void Register(object tickable);
        void Unregister(object tickable);
    }
}