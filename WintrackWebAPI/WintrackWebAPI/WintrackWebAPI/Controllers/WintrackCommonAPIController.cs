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
    /// WintrackCommonAPIController is the web api that loads the common api's. 
    /// For the current usage we use it to fill our dropdowns 
    /// WintrackBaseApiController inherited will help us with the exception handling
    /// </summary>
    public class WintrackCommonAPIController : WintrackBaseApiController
    {
        private readonly ICommonBO _wintrackBO = null;
        public WintrackCommonAPIController(ICommonBO commonBO)
        {
            _wintrackBO = commonBO;
        }

        /// <summary>
        /// A collection of service center entities is returned.
        /// In the current context it fills the Service center dropdown
        /// </summary>
        /// <returns>collection of ServiceCenterEntities</returns>
        [HttpGet]
        [Route(RouteConstants.GetServiceCenterDetails)]
        [ResponseType(typeof(List<ServiceCenterEntities>))]
        public IHttpActionResult GetServiceCenterDetails(bool useAll)
        {
            return Ok(_wintrackBO.GetServiceCenterDetails(useAll));
        }
        
        /// <summary>
        /// Returns the list of bank names with the account on the basis of the status passed
        /// in the current context this will fill the Bank Account dropdown
        /// </summary>
        /// <param name="bankAccountRequestModel"></param>
        [HttpPost]
        [Route(RouteConstants.GetBankAccountDetails)]
        [ResponseType(typeof(List<BankAccountEntities>))]
        public IHttpActionResult GetBankAccountDetails(BankAccountRequestModel bankAccountRequestModel)
        {
            return Ok(_wintrackBO.GetBankAccountDetails(bankAccountRequestModel));
        }

        /// <summary>
        /// Provides you the list of Billing types avaliable on the basis of the status passed
        /// </summary>
        /// <param name="billingTypeIdRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RouteConstants.GetBillingTypeDetails)]
        [ResponseType(typeof(List<BillingTypeEntities>))]
        public IHttpActionResult GetBillingTypeDetails(BillingTypeIdRequestModel billingTypeIdRequestModel)
        {
            return Ok(_wintrackBO.GetBillingTypeDetails(billingTypeIdRequestModel));
        }

        /// <summary>
        /// Returns the list of Clients based on the service center passed.
        /// </summary>
        /// <param name="clientDetailRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RouteConstants.GetClientDetails)]
        [ResponseType(typeof(List<ClientDetailEntities>))]
        public IHttpActionResult GetClientDetails(ClientDetailRequestModel clientDetailRequestModel)
        {
            return Ok(_wintrackBO.GetClientDetails(clientDetailRequestModel));
        }

    }
}
