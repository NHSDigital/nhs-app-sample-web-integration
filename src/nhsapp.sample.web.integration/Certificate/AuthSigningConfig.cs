using Microsoft.Extensions.Configuration;

namespace nhsapp.sample.web.integration.Certificate
{
    public interface IKeyConfig
    {
        string KeyPath { get; }
        string Password { get; }
    }
    public class AuthSigningConfig : IKeyConfig
    {
        public string KeyPath { get; }
        public string Password { get; }

        public AuthSigningConfig(IConfiguration configuration)
        {
            KeyPath = configuration["Auth:SigningKey"];
            Password = configuration["Auth:SigningPassword"];
        }
    }
}