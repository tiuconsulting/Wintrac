using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WintrackBO.Interface;
using WintrackEntities;
using WintrackExceptionHandler;
using WintrackWebAPI.App_Start;

namespace WintrackWebAPI.Controllers
{
    /// <summary>
    /// WintrackReportsAPIController is the controller to handle the R1017 report generation.
    /// WintrackBaseApiController inherited will help us with the exception handling
    /// </summary>
    public class WintrackReportsAPIController : WintrackBaseApiController
    {
        private readonly IWintrackReportsBO _wintrackReportsBO = null;
        public WintrackReportsAPIController(IWintrackReportsBO wintrackReportsBO)
        {
            _wintrackReportsBO = wintrackReportsBO;
        }
        /// <summary>
        /// This method used for generating the reports.
        /// </summary>
        /// <param name="reportingEntities">This is the object that provides the input params</param>
        /// <returns>Generate Id</returns>
        [HttpPost]
        [Route(RouteConstants.GenerateR1017Report)]
        [ResponseType(typeof(long))]
        public IHttpActionResult GenerateR1017Report(ReportingEntitiesRequestModel reportingEntities)
        {
            return Ok(_wintrackReportsBO.GenerateR1017Report(reportingEntities));
        }
    }
}
