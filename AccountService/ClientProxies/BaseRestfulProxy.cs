using Newtonsoft.Json;

namespace AccountService.ClientProxies
{
    /// <summary>
    /// Base Proxy for REST API calls
    /// </summary>
    public abstract class BaseRestfulProxy
    {
        private HttpClient _httpClient;
        protected HttpClient CreateOrGetHttpClient()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                _httpClient.Timeout = TimeSpan.FromMinutes(5);  //Set timeout to avoid too much resource cost.
            }
            return _httpClient;
        }

        /// <summary>
        /// Post Json Request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bodyContent"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<HttpResponseMessage> PostJsonRequestAsync(string url, object bodyContent)
        {
            HttpClient client = CreateOrGetHttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            string body = "";
            if (bodyContent != null)
            {
                if (bodyContent is string)
                    body = bodyContent.ToString();
                else
                    body = JsonConvert.SerializeObject(bodyContent);
            }
            request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
                throw new Exception("Error in Post Http Request, StatusCode: " + response.StatusCode + ", " + response.Content.ReadAsStringAsync());
        }
    }
}
