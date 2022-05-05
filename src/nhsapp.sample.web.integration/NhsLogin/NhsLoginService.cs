using System;
using System.Net;
using System.Threading.Tasks;
using nhsapp.sample.web.integration.NhsLogin.Models;
using nhsapp.sample.web.integration.TagHelpers;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public interface INhsLoginService
    {
        Task<GetUserProfileResult> GetUserProfile(string authCode, Uri redirectUrl);
    }

    public class NhsLoginService : INhsLoginService
    {
        private readonly INhsLoginClient _nhsLoginClient;
        private readonly IJwtTokenService<IdToken> _idTokenService;

        public NhsLoginService(
            INhsLoginClient nhsLoginClient,
            IJwtTokenService<IdToken> idTokenService
        )
        {

            _nhsLoginClient = nhsLoginClient;
            _idTokenService = idTokenService;
        }

        public async Task<GetUserProfileResult> GetUserProfile(string authCode, Uri redirectUrl)
        {
            var result = new GetUserProfileResult();

            var tokenResponse = await _nhsLoginClient.ExchangeAuthToken(authCode, redirectUrl);

            if (!tokenResponse.HasSuccessStatusCode)
            {
                result.StatusCode = MapNhsLoginErrorStatusCode(tokenResponse.StatusCode);
                result.UserProfile = Option.None<UserProfile>();
                return result;
            }

            var token = await _idTokenService.ReadToken(tokenResponse.Body.IdToken);

            await token.IfSome(async idToken =>
                {
                    result = await GetUserProfile(
                        tokenResponse.Body.AccessToken,
                        idToken.Subject,
                        tokenResponse.Body.RefreshToken);
                    result.IdTokenJti = idToken.Jti;
                })
                .IfNone(() =>
                {
                    result.StatusCode = HttpStatusCode.BadRequest;
                    result.UserProfile = Option.None<UserProfile>();
                    return Task.CompletedTask;
                });

            return result;

        }

        private async Task<GetUserProfileResult> GetUserProfile(string accessToken, string subject, string refreshToken = null)
        {
            var userInfoResponse = await _nhsLoginClient.GetUserInfo(accessToken);

            var validatedUserInfoResponse = ValidateUserInfoResponse(userInfoResponse);
            if (validatedUserInfoResponse.Failed(out var userInfoResponseFailure))
            {
                return userInfoResponseFailure;
            }

            var validatedUserInfo = ValidateUserInfo(validatedUserInfoResponse, subject);
            if (validatedUserInfo.Failed(out var validatedUserInfoFailure))
            {
                return validatedUserInfoFailure;
            }

            var userProfile = new UserProfile(validatedUserInfo, accessToken, refreshToken);

            return new GetUserProfileResult
            {
                StatusCode = HttpStatusCode.OK,
                UserProfile = Option.Some(userProfile)
            };
        }

        private ProcessResult<UserInfo, GetUserProfileResult> ValidateUserInfoResponse(
            NhsLoginApiObjectResponse<UserInfo> userInfoResponse)
        {
            if (userInfoResponse.HasSuccessStatusCode)
            {
                return userInfoResponse.Body;
            }

            return new GetUserProfileResult
            {
                StatusCode = HttpStatusCode.BadRequest,
                UserProfile = Option.None<UserProfile>()
            };
        }

        private ProcessResult<UserInfo, GetUserProfileResult> ValidateUserInfo(UserInfo userInfo, string subject)
        {
            if (!subject.Equals(userInfo.Subject, StringComparison.Ordinal))
            {
                return new GetUserProfileResult
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    UserProfile = Option.None<UserProfile>()
                };
            }
            return userInfo;
        }

        private static HttpStatusCode MapNhsLoginErrorStatusCode(HttpStatusCode code)
        {
            return code == HttpStatusCode.BadRequest
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.BadGateway;
        }
    }
}