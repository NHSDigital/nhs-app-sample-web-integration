using System;
using Microsoft.Extensions.Configuration;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public interface INhsLoginConfig
    {
        Uri NhsLoginApiBaseUrl { get; set; }
        string ClientId { get; set; }
        string Issuer { get; set; }
        string TokenPath { get; set; }
    }

    public class NhsLoginConfig : INhsLoginConfig
    {
        public Uri NhsLoginApiBaseUrl { get; set; }
        public string ClientId { get; set; }
        public string Issuer { get; set; }
        public string TokenPath { get; set; } = "token";

        public NhsLoginConfig(IConfiguration configuration)
        {
            NhsLoginApiBaseUrl = new Uri(configuration["NhsLogin:BaseUrl"]);
            ClientId = configuration["NhsLogin:ClientId"];
            Issuer = configuration["NhsLogin:JwtIssuer"];
        }
    }
}
