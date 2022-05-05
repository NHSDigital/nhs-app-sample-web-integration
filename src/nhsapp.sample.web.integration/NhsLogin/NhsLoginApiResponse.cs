using System.Net;
using nhsapp.sample.web.integration.NhsLogin.Models;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public class NhsLoginApiResponse
    {
        public HttpStatusCode StatusCode { get; }

        public ErrorResponse ErrorResponse { get; set; }

        public bool HasSuccessStatusCode => (int) StatusCode >= 200 && (int) StatusCode <= 299;

        protected NhsLoginApiResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}