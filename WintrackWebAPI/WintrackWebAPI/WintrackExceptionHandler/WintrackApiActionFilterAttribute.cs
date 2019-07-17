using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WintrackExceptionHandler
{
    public class WintrackApiActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //setting up the controller for future use to get the tokens or identify the user
            WintrackBaseApiController controller = (WintrackBaseApiController)actionContext.ControllerContext.Controller;

            //string user = Convert.ToString(controller.User);
        }
    }
}
