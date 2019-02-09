using Flurl;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HttpExtensions
{
    public static class RestHelper
    {
        private static HttpClient _client;

        public static HttpClient Client
        {
            get
            {
                if(_client == null)
                {
                    _client = new HttpClient();
                }

                return _client;
            }
        }

        public static AuthContext WithBasicAuth(this Url url, string username, string password)
        {
            string token = $@"{username}:{password}";

            byte[] tokenBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(token);

            string base64token = Convert.ToBase64String(tokenBytes);

            return new AuthContext(url, base64token, "Basic");
        }

        public static AuthContext WithOAuthBearerToken(this Url url, string token)
        {
            return new AuthContext(url, token, "Bearer");
        }

        public static async Task<HttpResponseMessage> PostJsonAsync(this AuthContext authContext, object requestBody)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string serializedRequestBody = JsonConvert.SerializeObject(requestBody, jsonSettings);

            HttpRequestMessage message = authContext
                .ToRequestMessage(HttpMethod.Post, new StringContent(serializedRequestBody));

            HttpResponseMessage response = await Client.SendAsync(message);

            return response;
        }

        public static async Task<T> GetJsonAsync<T>(this AuthContext authContext)
        {
            HttpRequestMessage message = authContext
                .ToRequestMessage(HttpMethod.Get);

            Task<HttpResponseMessage> responseTask = Client.SendAsync(message);

            T deserializedResponseContent = await responseTask.ReceiveJson<T>();

            return deserializedResponseContent;
        }

        public static async Task DeleteAsync(this AuthContext authContext)
        {
            HttpRequestMessage message = authContext
                .ToRequestMessage(HttpMethod.Delete);

            await Client.SendAsync(message);
        }

        public static async Task<T> ReceiveJson<T>(this Task<HttpResponseMessage> httpResponseMessageTask)
        {
            HttpResponseMessage httpResponseMessage = await httpResponseMessageTask;

            string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            var deserializedResponseContent = JsonConvert.DeserializeObject<T>(responseContent);

            return deserializedResponseContent;
        }

        private static HttpRequestMessage ToRequestMessage(this AuthContext authContext, HttpMethod method, HttpContent content = null)
        {
            HttpRequestMessage message = new HttpRequestMessage(method, authContext.Url);

            message.Headers.Authorization = new AuthenticationHeaderValue(authContext.Scheme, authContext.Token);

            if (content != null)
            {
                message.Content = content;

                message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }            

            return message;
        }
    }
}
