namespace nhsapp.sample.web.integration.NhsLogin
{
    public interface INhsLoginUriService
    {
        public string BuildLoginUri();
        public string BuildSsoLoginUri(string identityToken);
    }

    public class NhsLoginUriService:INhsLoginUriService
    {
        private readonly INhsLoginConfiguration _loginConfig;
        private readonly IAppWebConfiguration _webConfig;
        private readonly INhsLoginConfig _configuration;
        public NhsLoginUriService(INhsLoginConfiguration loginConfig, IAppWebConfiguration webConfig, INhsLoginConfig configuration)
        {
            _loginConfig = loginConfig;
            _webConfig = webConfig;
            _configuration = configuration;
        }

        public string BuildLoginUri()
        {
            var authoriseUri = GetBaseUri().Build();
            return authoriseUri.ToString();
        }

        public string BuildSsoLoginUri(string identityToken)
        {
            var authoriseUri = GetBaseUri()
                .AssertedLoginIdentity(identityToken)
                .Build();
            return authoriseUri.ToString();
        }

        private INhsLoginUriBuilder GetBaseUri()
        {
            return NhsLoginUriBuilder.Create(_loginConfig)
                .ClientId(_configuration.ClientId)
                .Scopes(NhsLoginScope.ScopeWithoutIm1)
                .VectorsOfTrust(NhsLoginVectorsOfTrust.P5BasicAndP9Sensitive)
                .RedirectUri(_webConfig.BaseAddress);
        }

    }
}