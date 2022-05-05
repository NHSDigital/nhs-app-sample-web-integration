using System;
using System.Collections.Generic;
using System.Linq;

namespace nhsapp.sample.web.integration.NhsLogin
{
    internal interface INhsLoginUriBuilder
    {
        INhsLoginUriBuilder Challenge(string challenge, string method);
        INhsLoginUriBuilder FidoAuthResponse(string? fidoAuthResponse);
        INhsLoginUriBuilder AssertedLoginIdentity(string token);
        INhsLoginUriBuilder State(string state);
        INhsLoginUriBuilder ClientId(string clientId);
        INhsLoginUriBuilder Scopes(NhsLoginScope scope);
        INhsLoginUriBuilder VectorsOfTrust(NhsLoginVectorsOfTrust vectorsOfTrust);
        INhsLoginUriBuilder RedirectUri(Uri redirectUri);
        Uri Build();
    }

    internal class NhsLoginUriBuilder : INhsLoginUriBuilder
    {
        private static readonly Dictionary<NhsLoginScope, string> ScopesRegister = new
            Dictionary<NhsLoginScope, string>
            {
                { NhsLoginScope.ScopeWithoutIm1, "openid profile profile_extended gp_registration_details email" },
                { NhsLoginScope.ScopeWithIm1, "openid profile profile_extended gp_registration_details email nhs_app_credentials" },
            };

        private static readonly Dictionary<NhsLoginVectorsOfTrust, string> VectorsOfTrustRegister = new
            Dictionary<NhsLoginVectorsOfTrust, string>
            {
                { NhsLoginVectorsOfTrust.P5Basic, "\"P5.Cp.Cd\", \"P5.Cp.Ck\", \"P5.Cm\"" },
                { NhsLoginVectorsOfTrust.P9Sensitive, "\"P9.Cp.Cd\", \"P9.Cp.Ck\", \"P9.Cm\"" },
                { NhsLoginVectorsOfTrust.P5BasicAndP9Sensitive, "\"P5.Cp.Cd\", \"P5.Cp.Ck\", \"P5.Cm\", \"P9.Cp.Cd\", \"P9.Cp.Ck\", \"P9.Cm\"" },
            };

        private readonly UriBuilder _uriBuilder;
        private readonly Dictionary<string, string> _queryString;

        private NhsLoginUriBuilder(INhsLoginConfiguration config)
        {
            _uriBuilder = new UriBuilder(config.AuthBaseAddress);

            _queryString = new Dictionary<string, string>
            {
                { "state", "A" },
                { "response_type", "code" }
            };
        }

        public static INhsLoginUriBuilder Create(INhsLoginConfiguration config)
        {
            return new NhsLoginUriBuilder(config);
        }

        public INhsLoginUriBuilder ClientId(string clientId)
        {
            _queryString.Add("client_id", clientId);
            return this;
        }

        public INhsLoginUriBuilder Scopes(NhsLoginScope scope)
        {
            _queryString.Add("scope", ScopesRegister[scope]);
            return this;
        }

        public INhsLoginUriBuilder VectorsOfTrust(NhsLoginVectorsOfTrust vectorsOfTrust)
        {
            _queryString.Add("vtr", $"[{VectorsOfTrustRegister[vectorsOfTrust]}]");
            return this;
        }

        public INhsLoginUriBuilder RedirectUri(Uri redirectUri)
        {
            _queryString.Add("redirect_uri", redirectUri.ToString());
            return this;
        }

        public INhsLoginUriBuilder FidoAuthResponse(string? fidoAuthResponse)
        {
            if (fidoAuthResponse != null)
            {
                _queryString.Add("fido_auth_response", fidoAuthResponse);
            }

            return this;
        }

        public INhsLoginUriBuilder Challenge(string challenge, string method)
        {
            _queryString.Add("code_challenge", challenge);
            _queryString.Add("code_challenge_method", method);
            return this;
        }

        public INhsLoginUriBuilder AssertedLoginIdentity(string token)
        {
            _queryString.Add("asserted_login_identity", token);
            return this;
        }

        public INhsLoginUriBuilder State(string state)
        {
            _queryString["state"] = state;
            return this;
        }

        public Uri Build()
        {
            var queryStringParts =
                _queryString.Select(kvp => $"{Uri.EscapeUriString(kvp.Key)}={Uri.EscapeUriString(kvp.Value)}");
            var queryString = string.Join("&", queryStringParts);

            _uriBuilder.Query = $"?{queryString}";

            return _uriBuilder.Uri;
        }
    }
}