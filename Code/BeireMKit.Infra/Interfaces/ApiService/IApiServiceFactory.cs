namespace BeireMKit.Infra.Interfaces.ApiService
{
    public interface IApiServiceFactory
    {
        IApiService Create(string clientName, string jwtTokenName = "JWT");
    }
}
