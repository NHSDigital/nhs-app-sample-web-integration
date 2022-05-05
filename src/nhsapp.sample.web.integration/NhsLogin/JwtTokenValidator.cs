using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public interface IJwtTokenValidator : ISecurityTokenValidator
    {
        public JwtSecurityToken ReadToken(string rawToken);
    }

    public class JwtTokenValidator : JwtSecurityTokenHandler, IJwtTokenValidator
    {
        public new JwtSecurityToken ReadToken(string rawToken) => base.ReadToken(rawToken) as JwtSecurityToken;
    }
}
