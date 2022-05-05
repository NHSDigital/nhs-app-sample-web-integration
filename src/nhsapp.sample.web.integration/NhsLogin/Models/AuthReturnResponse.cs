using Microsoft.AspNetCore.Mvc;

namespace nhsapp.sample.web.integration.NhsLogin.Models
{
    public class AuthReturnResponse
    {
        public  string Code { get; set; }

        public string State { get; set; }

        public string Error { get; set; }

        [FromQuery(Name = "error_description")]
        public string ErrorDescription { get; set; }
    }
}