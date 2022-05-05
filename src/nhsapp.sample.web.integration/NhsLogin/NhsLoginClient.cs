using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using nhsapp.sample.web.integration.NhsLogin.Models;
using nhsapp.sample.web.integration.ResponseParsers;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public interface INhsLoginClient
    {
        Task<NhsLoginApiObjectResponse<Token>> ExchangeAuthToken(string authCode, Uri redirectUrl);
        Task<NhsLoginApiObjectResponse<JsonWebKeySet>> GetSigningKeys();
        Task<NhsLoginApiObjectResponse<UserInfo>> GetUserInfo(string accessToken);

    }

    public class NhsLoginClient : INhsLoginClient
    {
        private const string SigningKeysPath = ".well-known/jwks.json";
        private const string UserInfoPath = "userinfo";

        private readonly NhsLoginHttpClient _httpClient;
        private readonly INhsLoginConfig _config;
        private readonly IJsonResponseParser _responseParser;
        private readonly INhsLoginJwtHelper _nhsLoginJwtHelper;

        public NhsLoginClient(
             NhsLoginHttpClient httpClient,
             INhsLoginConfig nhsLoginConfig,
             IJsonResponseParser responseParser,
             INhsLoginJwtHelper nhsLoginJwtHelper
            )
        {
             _httpClient = httpClient;
             _responseParser = responseParser;
             _config = nhsLoginConfig;
             _nhsLoginJwtHelper = nhsLoginJwtHelper;
        }

        public async Task<NhsLoginApiObjectResponse<Token>> ExchangeAuthToken(
            string authCode,
            Uri redirectUrl)
        {

            using (var request = new HttpRequestMessage(HttpMethod.Post, _config.TokenPath))
            {
                var token = _nhsLoginJwtHelper.CreateClientAuthJwt();

                var dict = new Dictionary<string, string>
                {
                    {"grant_type", "authorization_code"},
                    {"code", authCode},
                    {"redirect_uri", redirectUrl.ToString()},
                    {"client_assertion", token},
                    {"client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer"}
                };

                request.Content = new FormUrlEncodedContent(dict);

                var response = await SendRequestAndParseResponse<Token>(request);
                return response;
            }
        }

        public async Task<NhsLoginApiObjectResponse<UserInfo>> GetUserInfo(string accessToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, UserInfoPath))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await SendRequestAndParseResponse<UserInfo>(request);
                return response;
            }
        }

        public async Task<NhsLoginApiObjectResponse<JsonWebKeySet>> GetSigningKeys()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, SigningKeysPath))
            {
                var response = await SendRequestAndParseResponse<JsonWebKeySet>(request);
                return response;
            }
        }

        private async Task<NhsLoginApiObjectResponse<TResponse>> SendRequestAndParseResponse<TResponse>(
            HttpRequestMessage request)
        {
            var responseMessage = await _httpClient.Client.SendAsync(request);

            var response = new NhsLoginApiObjectResponse<TResponse>(responseMessage.StatusCode);

            var stringResponse = responseMessage.Content != null
                ? await responseMessage.Content.ReadAsStringAsync()
                : null;

            if (string.IsNullOrEmpty(stringResponse))
            {
                return response;
            }

            response.Body = _responseParser.ParseBody<TResponse>(stringResponse);
            response.ErrorResponse = _responseParser.ParseError<ErrorResponse>(stringResponse, responseMessage);

            return response;
        }
    }
}