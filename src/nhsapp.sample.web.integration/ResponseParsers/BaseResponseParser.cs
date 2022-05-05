using System.Linq;
using System.Net;
using System.Net.Http;

namespace nhsapp.sample.web.integration.ResponseParsers
{
    public interface IJsonResponseParser : IResponseParser
    {
    }

    public interface IResponseParser
    {
        T ParseBody<T>(string stringResponse);
        T ParseError<T>(string stringResponse,
            HttpResponseMessage message,
            params HttpStatusCode[] allowedErrorStatuses);
    }
    public abstract class BaseResponseParser: IResponseParser
    {
        public T ParseError<T>(string stringResponse,
            HttpResponseMessage message,
            params HttpStatusCode[] allowedErrorStatuses)
        {
            if (message.IsSuccessStatusCode || allowedErrorStatuses.Any(x => x == message.StatusCode))
            {
                return default;
            }

            return ParseBody<T>(stringResponse);
        }

        public abstract T ParseBody<T>(string stringResponse);
    }
}