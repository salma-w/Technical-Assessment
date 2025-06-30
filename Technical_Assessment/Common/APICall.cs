using System.Text;
using System.Text.Json;

namespace Common
{
    /// <summary>
    /// this method is used to send text to an API endpoint.it is used in the console app and unnit test app
    /// </summary>
    public class APICall
    {
        public static async Task<string> SendTextToApiAsync(HttpClient client, string endpointUrl, string text)
        {
            var payload = new { text };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpointUrl, content);
            response.EnsureSuccessStatusCode(); // throw if non-2xx
            return await response.Content.ReadAsStringAsync();
        }
    }
}
