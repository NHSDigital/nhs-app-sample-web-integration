namespace nhsapp.sample.web.integration.NhsLogin.Models
{
    public class UserProfile
    {
        private readonly UserInfo _userInfo;

        public UserProfile(UserInfo userInfo, string accessToken, string refreshToken)
        {
            _userInfo = userInfo;
            AccessToken = accessToken;
        }
        public string DateOfBirth => _userInfo.Birthdate;
        public string NhsNumber => _userInfo.NhsNumber;
        public string GivenName => _userInfo.GivenName;
        public string FamilyName => _userInfo.FamilyName;
        public string IdentityProofingLevel => _userInfo.IdentityProofingLevel;
        public string Email => _userInfo.Email;
        public string AccessToken { get; }

    }
}
