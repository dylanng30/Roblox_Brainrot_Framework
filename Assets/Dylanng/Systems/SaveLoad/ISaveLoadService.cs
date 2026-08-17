using System;

namespace Dylanng
{
    public interface ISaveLoadService<T>
    {
        bool Exist(string keyName);
        T Load(string keyName);
        void Save(T data, string keyName);
    }
}
