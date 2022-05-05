using System;
using Microsoft.Extensions.Configuration;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public interface IAppWebConfiguration
    {
        string BaseUrl { get; }
        string AuthorizeReturnPath { get; }
        Uri BaseAddress => new(new Uri(BaseUrl), AuthorizeReturnPath);

    }
    public class AppWebConfiguration : IAppWebConfiguration
    {
        private readonly IConfiguration _configuration;

        public AppWebConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string BaseUrl => _configuration["WebConfiguration:BaseUrl"];
        public string AuthorizeReturnPath => _configuration["WebConfiguration:AuthorizeReturnPath"];
    }
}