namespace Dylanng
{
    public interface IService { }

    public interface IManager : IService
    {
        void Initialize();
    }

    public interface ISystem : IService
    {
        void Cleanup();
    }
    
}