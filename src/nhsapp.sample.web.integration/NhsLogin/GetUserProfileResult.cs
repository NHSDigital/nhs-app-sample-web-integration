using System.Net;
using nhsapp.sample.web.integration.NhsLogin.Models;
using nhsapp.sample.web.integration.TagHelpers;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public class GetUserProfileResult
    {
        public Option<UserProfile> UserProfile { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string IdTokenJti { get; set; }
    }
}