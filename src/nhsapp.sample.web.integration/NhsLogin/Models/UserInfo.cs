using System;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;

namespace nhsapp.sample.web.integration.NhsLogin.Models
{
    [Serializable]
    public class UserInfo
    {
        [JsonProperty(JwtRegisteredClaimNames.GivenName)]
        public string GivenName { get; set; }

        [JsonProperty(JwtRegisteredClaimNames.FamilyName)]
        public string FamilyName { get; set; }

        [JsonProperty("gp_registration_details")]
        public GpRegistrationDetails GpRegistrationDetails { get; } = new GpRegistrationDetails();

        [JsonProperty("nhs_number")]
        public string NhsNumber { get; set; }

        [JsonProperty("im1_token")]
        public string Im1ConnectionToken { get; set; }

        [JsonProperty(JwtRegisteredClaimNames.Sub)]
        public string Subject { get; set; }

        [JsonProperty(JwtRegisteredClaimNames.Birthdate)]
        public string Birthdate { get; set; }

        [JsonProperty("identity_proofing_level")]
        public string IdentityProofingLevel { get; set; }

        public string Email { get; set; }
    }
}
