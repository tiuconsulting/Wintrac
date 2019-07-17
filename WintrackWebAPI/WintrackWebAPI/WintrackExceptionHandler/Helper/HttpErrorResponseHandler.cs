using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WintrackEntities.Error;

namespace WintrackExceptionHandler.Helper
{
    /// <summary>
    /// Error Status Code to be returned in the response.
    /// </summary>
    public static class HttpErrorResponseHandler
    {
        public static HttpStatusCode GetHttpErrorResponseCode(WintrackValidationException validationException)
        {
            if (validationException.ErrorCode == "AccessDenied")
            {
                return HttpStatusCode.Unauthorized;
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
        }
    }
}
