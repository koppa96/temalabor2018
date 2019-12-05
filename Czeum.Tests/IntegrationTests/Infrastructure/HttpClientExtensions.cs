using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;

namespace Czeum.Tests.IntegrationTests.Infrastructure
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostJsonAsync(this HttpClient client, string uri, object obj, string user = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Add("Authorization", user);
            request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            return client.SendAsync(request);
        }

        public static async Task<T> GetJsonAsync<T>(this HttpClient client, string uri, string user = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Authorization", user);

            var response = await client.SendAsync(request);
            response.IsSuccessStatusCode.Should().Be(true);

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}