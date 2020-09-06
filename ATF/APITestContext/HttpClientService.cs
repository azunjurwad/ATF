using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ATF.APITestContext
{
    public class HttpClientService : HttpClient
    {
        private static readonly HttpClientService instance = new HttpClientService();

        private HttpResponseMessage lastResponse;

        private string lastResponseContent;

        public HttpClientService() : base()
        {
            Timeout = TimeSpan.FromSeconds(15);
            MaxResponseContentBufferSize = 256000;
        }

        public static HttpClientService Instance
        {
            get
            {
                return instance;
            }
        }

        public HttpResponseMessage LastResponse
        {
            get
            {
                return lastResponse;
            }
            set
            {
                lastResponse = value;

                if (lastResponse != null)
                {
                    lastResponse.Content.LoadIntoBufferAsync().Wait();
                    lastResponseContent = lastResponse.Content.ReadAsStringAsync().Result;
                }
                else
                    lastResponse = null;

            }
        }

        public string LastResponseContent
        {
            get
            {
                return lastResponseContent;
            }
            set
            {
                lastResponseContent = value;
            }
        }

        public HttpResponseMessage GetLastResponse(string baseUri, string request)
        {
            if (Instance.BaseAddress == null)
                Instance.BaseAddress = new Uri(baseUri);
            LastResponse = Instance.GetAsync(request).Result;
            return LastResponse;
        }

        public T GetLastResponseContentAs<T>(string baseUri, string request)
        {
            GetLastResponse(baseUri, request);
            return JsonConvert.DeserializeObject<T>(LastResponseContent);
        }

        public string GetLastResponseResultAsString(string baseUri, string request)
        {
            LastResponse = GetLastResponse(baseUri, request);
            var lastContent = LastResponse.Content.ReadAsStringAsync();
            var lastResult = lastContent.Result;
            return lastResult;
        }

        public async Task<string> GetLastResponseContentAsync(string baseUri, string request)
        {
            LastResponse = GetLastResponse(baseUri, request);
            LastResponseContent = await LastResponse.Content.ReadAsStringAsync();
            return LastResponseContent;
        }

        public void EnsureSuccess()
        {
            if ((int)LastResponse.StatusCode >= 400)
            {
                throw new WebException($"Web request failed. StatusCode:{LastResponse.StatusCode} ReasonPhrase:{LastResponse.ReasonPhrase} ResponseContent:{LastResponseContent}");
            }
        }

        public async Task<HttpResponseMessage> PostPayload<T>(string baseUri, string request, T payload)
        {
            if (Instance.BaseAddress == null)
                Instance.BaseAddress = new Uri(baseUri);
            var json = JsonConvert.SerializeObject(payload);
            var contents = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await Instance.PostAsync(request, contents);
            if (httpResponse.IsSuccessStatusCode)
            {
                return httpResponse;
            }
            throw new Exception(httpResponse.ReasonPhrase);
        }

        public async Task<HttpResponseMessage> PostAuth(string request, FormUrlEncodedContent payload)
        {
            HttpClient httpClient = new HttpClient();
            var httpResponse = await httpClient.PostAsync(request, payload);
            if (httpResponse.IsSuccessStatusCode)
            {
                return httpResponse;
            }
            throw new Exception(httpResponse.ReasonPhrase);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string baseUri, string request, T payload)
        {
            if (Instance.BaseAddress == null)
                Instance.BaseAddress = new Uri(baseUri);
            var json = JsonConvert.SerializeObject(payload);
            var contents = new StringContent(json, Encoding.UTF8, "application/json");
            LastResponse = await Instance.PutAsync(request, contents);
            if (LastResponse.IsSuccessStatusCode)
            {
                return LastResponse;
            }
            throw new Exception(LastResponse.ReasonPhrase);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string baseUri, string request)
        {
            if (Instance.BaseAddress == null)
                Instance.BaseAddress = new Uri(baseUri);
            LastResponse = await Instance.DeleteAsync(request);
            if (LastResponse.IsSuccessStatusCode)
            {
                return LastResponse;
            }
            throw new Exception(LastResponse.ReasonPhrase);
        }
    }
}
