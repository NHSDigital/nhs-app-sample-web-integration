using System.Net.Http;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public class NhsLoginHttpClient
    {
        public NhsLoginHttpClient(HttpClient client, INhsLoginConfig config)
        {
            client.BaseAddress = config.NhsLoginApiBaseUrl;
            Client = client;
        }

        public HttpClient Client { get; }
    }
}
