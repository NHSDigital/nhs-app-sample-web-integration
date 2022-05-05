using System.Net;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public class NhsLoginApiObjectResponse<TBody> : NhsLoginApiResponse
    {
        public NhsLoginApiObjectResponse(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public TBody Body { get; set; }
    }
}