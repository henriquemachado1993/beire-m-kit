namespace BeireMKit.Infra.Interfaces.ApiService
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string route);
        Task<T> PostAsync<T>(string route, object data);
        Task<T> PutAsync<T>(string route, object data);
        Task<T> DeleteAsync<T>(string route);
    }
}
