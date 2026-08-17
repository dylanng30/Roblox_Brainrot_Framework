namespace Dylanng
{
    public interface ISaveLoadManager : IManager
    {
        void RegisterRepository<T>(ISaveLoadService<T> repository);
        void UnregisterRepository<T>();
        void Save<T>(T data, string keyName);
        T Load<T>(string keyName);
        bool Exist(string keyName);
        bool Exist<T>(string keyName);
        void Delete(string keyName);
    }
}