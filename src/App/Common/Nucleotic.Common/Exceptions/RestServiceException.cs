using System;
using System.Net;
using System.Runtime.Serialization;

namespace Nucleotic.Common.Exceptions
{
    public class RestServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public RestServiceException(HttpStatusCode code)
        {
            StatusCode = code;
        }

        public RestServiceException(HttpStatusCode code, string message) : base(message)
        {
            StatusCode = code;
        }

        public RestServiceException(HttpStatusCode code, string message, Exception inner) : base(message, inner)
        {
            StatusCode = code;
        }

        protected RestServiceException(HttpStatusCode code, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = code;
        }
    }
}
