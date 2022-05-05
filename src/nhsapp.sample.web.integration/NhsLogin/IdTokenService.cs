using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using nhsapp.sample.web.integration.NhsLogin.Models;
using nhsapp.sample.web.integration.TagHelpers;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public interface IJwtTokenService<T>
    {
        Task<Option<IdToken>> ReadToken(string token);
    }

    public class IdTokenService : IJwtTokenService<IdToken>
    {
        private readonly ITokenValidationParameterBuilder _parameterBuilder;
        private readonly IJwtTokenValidator _jwtTokenHandler;
        private readonly INhsLoginSigningKeysProvider _nhsLoginKeysProvider;

        public IdTokenService(ITokenValidationParameterBuilder parameterBuilder,
            IJwtTokenValidator tokenValidator,
            INhsLoginSigningKeysProvider nhsLoginKeysProvider
            )
        {
            _jwtTokenHandler = tokenValidator;
            _nhsLoginKeysProvider = nhsLoginKeysProvider;
            _parameterBuilder = parameterBuilder;
        }

        public async Task<Option<IdToken>> ReadToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return Option.None<IdToken>();
            }

            if (!_jwtTokenHandler.CanReadToken(token))
            {
                return Option.None<IdToken>();
            }

            if (!_jwtTokenHandler.CanValidateToken)
            {
                return Option.None<IdToken>();
            }

            try
            {
                var tokenKeyId = _jwtTokenHandler.ReadToken(token).Header.Kid;
                var signingKeysResponse = await _nhsLoginKeysProvider.GetSigningKeys(tokenKeyId);

                if (!signingKeysResponse.HasValue)
                {
                    return Option.None<IdToken>();
                }

                var signingKeys = signingKeysResponse.ValueOrFailure();
                var validationParameters = _parameterBuilder.Build(signingKeys);

                var principal = _jwtTokenHandler.ValidateToken(token, validationParameters, out _);

                var subject = principal.FindFirstValue(ClaimTypes.NameIdentifier);
                if (subject == null)
                {
                    return Option.None<IdToken>();
                }

                var jti = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
                if (jti == null)
                {
                    return Option.None<IdToken>();
                }

                var exp = principal.FindFirstValue(JwtRegisteredClaimNames.Exp);

                return Option.Some(new IdToken { Subject = subject, Jti = jti });
            }
            catch
            {
                return Option.None<IdToken>();
            }
        }
    }
}