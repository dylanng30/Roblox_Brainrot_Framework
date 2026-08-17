namespace Dylanng
{
    public abstract class SystemBase : ISystem
    {
        public abstract void Initialize();
        public abstract void Cleanup();
    }
}