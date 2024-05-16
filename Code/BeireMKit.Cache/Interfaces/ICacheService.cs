namespace BeireMKit.Cache.Interfaces
{
    public interface ICacheService
    {
        bool KeyExists(string key);
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan expiration);
        void Remove(string key);
    }
}
