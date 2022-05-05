using System;
using Newtonsoft.Json;

namespace nhsapp.sample.web.integration.NhsLogin.Models
{
    [Serializable]
    public class GpRegistrationDetails
    {
        [JsonProperty("gp_ods_code")]
        public string OdsCode { get; set; }
    }
}
