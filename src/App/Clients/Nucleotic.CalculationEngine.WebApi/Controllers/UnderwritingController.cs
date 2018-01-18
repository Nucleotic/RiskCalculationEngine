using Nucleotic.DataContracts.CalculationEngine.Model.Services;
using Nucleotic.Module.CalculationEngine.Controllers;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Nucleotic.CalculationEngine.WebApi.Controllers
{
    /// <summary>
    /// Nucleotic Rating Engine underwriting resource
    /// </summary>
    /// <seealso cref="CommonApiController" />
    [RoutePrefix("api/underwriting")]
    public class UnderwritingController : CommonApiController
    {
        private readonly UnderwritingCalculationsController _calculationsController;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnderwritingController"/> class.
        /// </summary>
        public UnderwritingController()
        {
            _calculationsController = new UnderwritingCalculationsController();
        }

        /// <summary>
        /// Calculates the motor loadings.
        /// </summary>
        /// <param name="request">The vehicle calculation request.</param>
        /// <returns>HttpResponseMessage containing the data</returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>
        [Route("motorloadings")]
        [HttpPost]
        [ResponseType(typeof(CalculationResponse))]
        public HttpResponseMessage CalculateMotorLoadings([FromBody] VehicleCalculationRequest request)
        {
            HttpResponseMessage response;
            try
            {
                var results = _calculationsController.CalculateMotorLoadings(request, request.UseState);
                response = Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return response;
        }

        /// <summary>
        /// Calculates the building loadings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("buildingloadings")]
        [HttpPost]
        [ResponseType(typeof(CalculationResponse))]
        public HttpResponseMessage CalculateBuildingLoadings([FromBody] BuildingCalculationRequest request)
        {
            HttpResponseMessage response;
            try
            {
                var results = _calculationsController.CalculateBuildingLoadings(request, request.UseState);
                response = Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return response;
        }

        /// <summary>
        /// Calculates the contents loadings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("contentsloadings")]
        [HttpPost]
        [ResponseType(typeof(CalculationResponse))]
        public HttpResponseMessage CalculateContentsLoadings([FromBody] ContentsCalculationRequest request)
        {
            HttpResponseMessage response;
            try
            {
                var results = _calculationsController.CalculateContentsLoadings(request, request.UseState);
                response = Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return response;
        }

        /// <summary>
        /// Calculates all risk loadings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("allriskloadings")]
        [HttpPost]
        [ResponseType(typeof(CalculationResponse))]
        public HttpResponseMessage CalculateAllRiskLoadings([FromBody] AllRiskCalculationRequest request)
        {
            HttpResponseMessage response;
            try
            {
                var results = _calculationsController.CalculateAllRiskLoadings(request, request.UseState);
                response = Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return response;
        }

        /// <summary>
        /// Calculates the excess waiver.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("excesswaiver")]
        [HttpPost]
        [ResponseType(typeof(CalculationResponse))]
        public HttpResponseMessage CalculateExcessWaiver([FromBody] ExcessWaiverCalculationRequest request)
        {
            HttpResponseMessage response;
            try
            {
                var results = _calculationsController.CalculateExcessWaiver(request, request.UseState);
                response = Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return response;
        }

        /// <summary>
        /// Calculates the flat rate loading.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("flatrate")]
        [HttpPost]
        [ResponseType(typeof(CalculationResponse))]
        public HttpResponseMessage CalculateFlatRateLoading([FromBody] FlatRateCalculationRequest request)
        {
            HttpResponseMessage response;
            try
            {
                var results = _calculationsController.CalculateFlatRateLoadings(request, request.UseState);
                response = Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return response;
        }
    }
}