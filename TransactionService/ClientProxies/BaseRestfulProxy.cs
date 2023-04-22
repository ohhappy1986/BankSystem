namespace TransactionService.ClientProxies
{
    /// <summary>
    /// Base proxy for REST API calls.
    /// </summary>
    public abstract class BaseRestfulProxy
    {
        private HttpClient _httpClient;
        protected HttpClient CreateOrGetHttpClient()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                _httpClient.Timeout = TimeSpan.FromMinutes(5);
            }
            return _httpClient;
        }

        public virtual async Task<HttpResponseMessage> PutRequestWithoutBodyAsync(string url)
        {
            HttpClient client = CreateOrGetHttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
                throw new Exception("Error in Put Http Request without body, StatusCode: " + response.StatusCode + ", " + response.Content.ReadAsStringAsync());
        }

        public virtual async Task<HttpResponseMessage> GetRequestAsync(string url)
        {
            HttpClient client = CreateOrGetHttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
                throw new Exception("Error in Get Http Request without body, StatusCode: " + response.StatusCode + ", " + response.Content.ReadAsStringAsync());
        }
    }
}
