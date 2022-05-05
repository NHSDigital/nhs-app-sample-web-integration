using System;
using Microsoft.Extensions.Configuration;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public interface INhsLoginConfiguration
    {
        string BaseUrl { get; }
        string AuthorizePath { get; }
        Uri AuthBaseAddress => new Uri($"{BaseUrl}{AuthorizePath}");
    }
    public class NhsLoginConfiguration : INhsLoginConfiguration
    {
        private readonly IConfiguration _configuration;
        public NhsLoginConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string BaseUrl => _configuration["NhsLogin:BaseUrl"];

        public string AuthorizePath => _configuration["NhsLogin:AuthorizePath"];
    }
}