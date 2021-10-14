using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Contracts.Requests;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!\n\n");
            var testClient = new TestClient();
            // Console.WriteLine(await testClient.GetResponses());
            await testClient.SendReponses();
            Console.ReadLine();
        }
    }

    public class TestClient
    {
        private HttpClient _client;
        private const string BaseUrl = "https://localhost:5001";

        public TestClient()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            _client = new HttpClient(handler);
        }

        public async Task<string> GetResponses()
        {
            var result = await _client.GetAsync(BaseUrl);
            return await result.Content.ReadAsStringAsync();
        }

        public async Task SendReponses()
        {
            var content = new IndexRequest
            {
                Content = "Testing content"
            };

            using var result = await _client.PostAsync(BaseUrl, new JsonContent(content));
            var stringResult = await result.Content.ReadAsStringAsync();
            Console.WriteLine(stringResult);
        }

        ~TestClient()
        {
            _client.Dispose();
        }
    }

    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json") { }
    }
}
