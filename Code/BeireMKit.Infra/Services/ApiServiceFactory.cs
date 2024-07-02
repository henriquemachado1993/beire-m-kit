using BeireMKit.Infra.Interfaces.ApiService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BeireMKit.Infra.Services
{
    public class ApiServiceFactory : IApiServiceFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ApiServiceFactory(IHttpContextAccessor httpContextAccessor,
                                 IHttpClientFactory httpClientFactory,
                                 IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        /// <summary>
        /// Configuration on BaseUrl ApiService.ClienName.BaseUrl
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="jwtTokenName"></param>
        /// <returns></returns>
        public IApiService Create(string clientName, string jwtTokenName = "JWT")
        {
            if(string.IsNullOrWhiteSpace(clientName))
                throw new ArgumentNullException("you need to enter the clientName");

            var configuration = $"ApiService:{clientName}:BaseUrl";
            var baseUrl = _configuration[$"ApiService:{clientName}:BaseUrl"] ?? throw new ArgumentNullException($"The base URL was not found in the '{configuration}' path.");
            var client = _httpClientFactory.CreateClient(clientName);
            client.BaseAddress = new Uri(baseUrl);

            return new ApiService(client, _httpContextAccessor, jwtTokenName);
        }
    }
}
