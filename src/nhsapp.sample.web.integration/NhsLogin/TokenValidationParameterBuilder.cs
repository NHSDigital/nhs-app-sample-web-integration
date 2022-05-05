using Microsoft.IdentityModel.Tokens;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public interface ITokenValidationParameterBuilder
    {
        TokenValidationParameters Build(JsonWebKeySet keys);
    }

    public class TokenValidationParameterBuilder : ITokenValidationParameterBuilder
    {
        private readonly string _issuer;
        private readonly string _audience;

        public TokenValidationParameterBuilder(INhsLoginConfig config)
        {
            _issuer = config.Issuer;
            _audience = config.ClientId;
        }

        public TokenValidationParameters Build(JsonWebKeySet keys)
        {
            return new TokenValidationParameters
            {
                IssuerSigningKeys = keys.GetSigningKeys(),
                ValidAudience = _audience,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                RequireExpirationTime = true,
                ValidateLifetime = true
            };
        }
    }
}