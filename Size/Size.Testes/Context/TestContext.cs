using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Size.Api;
using Microsoft.Extensions.Configuration;

namespace Size.Testes.Context
{
    public class TestContext
    {
        public HttpClient Client { get; set; }
        private readonly TestServer _server;

        public TestContext()
        {
            _server = new TestServer(new WebHostBuilder().UseConfiguration(
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<Startup>());
            Client = _server.CreateClient();
        }
    }
}
