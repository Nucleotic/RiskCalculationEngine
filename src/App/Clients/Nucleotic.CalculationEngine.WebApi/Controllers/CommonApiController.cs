using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Nucleotic.CalculationEngine.WebApi.Controllers
{
    /// <summary>
    /// Common API resource messages
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public abstract class CommonApiController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="System.Web.Http.IHttpActionResult" />
        public class ForbiddenResult : IHttpActionResult
        {
            private readonly HttpRequestMessage _request;
            private readonly string _reason;

            /// <summary>
            /// Initializes a new instance of the <see cref="ForbiddenResult"/> class.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="reason">The reason.</param>
            public ForbiddenResult(HttpRequestMessage request, string reason)
            {
                _request = request;
                _reason = reason;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ForbiddenResult"/> class.
            /// </summary>
            /// <param name="request">The request.</param>
            public ForbiddenResult(HttpRequestMessage request)
            {
                _request = request;
                _reason = "Forbidden";
            }

            /// <summary>
            /// Creates an <see cref="T:System.Net.Http.HttpResponseMessage" /> asynchronously.
            /// </summary>
            /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
            /// <returns>
            /// A task that, when completed, contains the <see cref="T:System.Net.Http.HttpResponseMessage" />.
            /// </returns>
            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = _request.CreateResponse(HttpStatusCode.Forbidden, _reason);
                return Task.FromResult(response);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="System.Web.Http.IHttpActionResult" />
        public class NoContentResult : IHttpActionResult
        {
            private readonly HttpRequestMessage _request;
            private readonly string _reason;

            /// <summary>
            /// Initializes a new instance of the <see cref="NoContentResult"/> class.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="reason">The reason.</param>
            public NoContentResult(HttpRequestMessage request, string reason)
            {
                _request = request;
                _reason = reason;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="NoContentResult"/> class.
            /// </summary>
            /// <param name="request">The request.</param>
            public NoContentResult(HttpRequestMessage request)
            {
                _request = request;
                _reason = "No Content";
            }

            /// <summary>
            /// Creates an <see cref="T:System.Net.Http.HttpResponseMessage" /> asynchronously.
            /// </summary>
            /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
            /// <returns>
            /// A task that, when completed, contains the <see cref="T:System.Net.Http.HttpResponseMessage" />.
            /// </returns>
            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = _request.CreateResponse(HttpStatusCode.NoContent, _reason);
                return Task.FromResult(response);
            }
        }
    }
}