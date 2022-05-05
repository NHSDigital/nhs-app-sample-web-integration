using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace nhsapp.sample.web.integration.ResponseParsers
{
    public class JsonResponseParser : BaseResponseParser, IJsonResponseParser
    {
        public override T ParseBody<T>(string stringResponse)
        {
            try
            {
                var serializedResponse = Deserialize<T>(stringResponse);

                if (serializedResponse != null)
                {
                    return  serializedResponse;
                }

                return default;
            }
            catch (JsonException exception)
            {
                throw new NhsUnparsableException("Response parsing failed.", exception);
            }
        }

        private T Deserialize<T>(string stringResponse)
        {
            var errors = new List<ErrorContext>();

            var response = JsonConvert.DeserializeObject<T>(stringResponse,
                new JsonSerializerSettings
                {
                    Error = delegate(object sender, ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext);
                        args.ErrorContext.Handled = true;
                    }
                });

            if (errors.Any())
            {
                throw new NhsUnparsableException("Response parsing failed.");
            }

            return response;
        }
    }
}