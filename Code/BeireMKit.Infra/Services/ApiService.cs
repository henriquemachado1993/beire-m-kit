using BeireMKit.Infra.Interfaces.ApiService;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BeireMKit.Infra.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _jwtTokenName;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, string jwtTokenName = "JWT")
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _jwtTokenName = jwtTokenName ?? throw new ArgumentNullException(nameof(jwtTokenName));
        }

        public async Task<T> GetAsync<T>(string route)
        {
            AddAuthorizationHeader();

            var response = await _httpClient.GetAsync(route);
            return await HandleResponse<T>(response);
        }

        public async Task<T> PostAsync<T>(string route, object data)
        {
            AddAuthorizationHeader();

            var response = await _httpClient.PostAsJsonAsync(route, data);
            return await HandleResponse<T>(response);
        }

        public async Task<T> PutAsync<T>(string route, object data)
        {
            AddAuthorizationHeader();

            var response = await _httpClient.PutAsJsonAsync(route, data);
            return await HandleResponse<T>(response);
        }

        public async Task<T> DeleteAsync<T>(string route)
        {
            AddAuthorizationHeader();

            var response = await _httpClient.DeleteAsync(route);
            return await HandleResponse<T>(response);
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext.Items[_jwtTokenName]?.ToString();
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("You need to set the authentication token via the middleware to httpContextAccessor.httpContext.Items[JwtTokenName]");
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
            }
        }
    }
}
