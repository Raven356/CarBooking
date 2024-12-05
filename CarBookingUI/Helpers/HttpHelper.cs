using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CarBookingUI.Helpers
{
    internal static class HttpHelper
    {
        private static readonly HttpClient _httpClient;

        static HttpHelper()
        {
            _httpClient = new HttpClient(InsecureHandler.InsecureHandler.GetInsecureHandler());
        }

        public static HttpResponseMessage Get(string requestUrl)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecureStorage.GetAsync("auth_token").GetAwaiter().GetResult());
            var response = _httpClient.GetAsync(requestUrl).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                SetNewAuthTokenAsync(response.Headers).GetAwaiter();
            }

            return response;
        }

        public static async Task<HttpResponseMessage> GetAsync(string requestUrl)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                await SetNewAuthTokenAsync(response.Headers);
            }

            return response;
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUrl, T body)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var response = await _httpClient.PostAsJsonAsync(requestUrl, body);

            if (response.IsSuccessStatusCode) 
            {
                await SetNewAuthTokenAsync(response.Headers);
            }

            return response;
        }

        public static async Task<HttpResponseMessage> PostAsync(string requestUrl)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var response = await _httpClient.PostAsync(requestUrl, null);

            if (response.IsSuccessStatusCode)
            {
                await SetNewAuthTokenAsync(response.Headers);
            }

            return response;
        }

        private static async Task SetNewAuthTokenAsync(HttpResponseHeaders headers) 
        {
            if (headers.TryGetValues("New-Access-Token", out var tokenValues))
            {
                // Get the token value (assuming only one value is returned)
                string newAccessToken = tokenValues.FirstOrDefault();
                await SecureStorage.SetAsync("auth_token", newAccessToken);
            }
        }
    }
}
