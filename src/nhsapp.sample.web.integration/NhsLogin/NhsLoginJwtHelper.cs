using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using nhsapp.sample.web.integration.Certificate;
using nhsapp.sample.web.integration.Jwt;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public interface INhsLoginJwtHelper
    {
        string CreateClientAuthJwt();
        string CreateAssertedLoginIdentityJwt(string idTokenJti);
    }

    public class NhsLoginJwtHelper : INhsLoginJwtHelper
    {
        private readonly RSAParameters _rsaParams;
        private readonly INhsLoginConfig _nhsLoginConfig;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly string _audience;

        public NhsLoginJwtHelper(
            INhsLoginConfig nhsLoginConfig,
            AuthSigningConfig authSigningConfig,
            IJwtTokenGenerator jwtTokenGenerator,
            ISigning signing)
        {
            _nhsLoginConfig = nhsLoginConfig;
            _jwtTokenGenerator = jwtTokenGenerator;

            _rsaParams = signing.GetRsaParameters(authSigningConfig);
            _audience = BuildAudience(nhsLoginConfig);
        }

        private static string BuildAudience(INhsLoginConfig config)
        {
            var audienceBuilder = new UriBuilder(config.Issuer) { Path = config.TokenPath };
            return audienceBuilder.Uri.ToString();
        }


        public string CreateAssertedLoginIdentityJwt(string idTokenJti)
        {
            var payload = new Dictionary<string, object>
            {
                { "code", idTokenJti },
                { JwtRegisteredClaimNames.Iss, _nhsLoginConfig.ClientId },
                { JwtRegisteredClaimNames.Jti, Guid.NewGuid() },
                { JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds() },
                { JwtRegisteredClaimNames.Exp, DateTimeOffset.Now.AddMinutes(1).ToUnixTimeSeconds() }
            };

            return _jwtTokenGenerator.GenerateJwtSecurityToken(_rsaParams, payload);
        }

        public string CreateClientAuthJwt()
        {

            var payload = new Dictionary<string, object>
            {
                {JwtRegisteredClaimNames.Sub, _nhsLoginConfig.ClientId},
                {JwtRegisteredClaimNames.Aud, _audience},
                {JwtRegisteredClaimNames.Iss, _nhsLoginConfig.ClientId},
                {JwtRegisteredClaimNames.Exp, DateTimeOffset.Now.AddMinutes(1).ToUnixTimeSeconds()},
                {JwtRegisteredClaimNames.Jti, Guid.NewGuid()}
            };

            return _jwtTokenGenerator.GenerateJwtSecurityToken(_rsaParams, payload);
        }
    }
}