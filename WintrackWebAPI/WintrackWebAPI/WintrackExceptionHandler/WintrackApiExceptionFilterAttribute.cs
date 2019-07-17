using System;
using System.Reflection;
using System.Net;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using WintrackExceptionHandler.Helper;
using WintrackExceptionHandler.Entities;
using WintrackEntities.Error;

namespace WintrackExceptionHandler
{
    public class WintrackApiExceptionFilterAttribute: ExceptionFilterAttribute
    {
        

        #region Public Methods
        public WintrackApiExceptionFilterAttribute()
        {
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                if (actionExecutedContext.Exception != null)
                {
                    string controllerWarning = string.Empty;
                    var actionDesc = ((ReflectedHttpActionDescriptor)actionExecutedContext.ActionContext.ActionDescriptor);
                    var requestHeader = actionExecutedContext.ActionContext.Request.Headers;
                   
                    
                    IHttpController controller = actionExecutedContext.ActionContext.ControllerContext.Controller;
                    string apiMethodArguments = JsonConvert.SerializeObject(actionExecutedContext.ActionContext.ActionArguments);

                    string consolidatedErrorMessage = string.Format("Controller: {0};\nAction: {1};\nParams: {2};\nTrace: {3}",
                        actionDesc.ControllerDescriptor.ControllerName, actionDesc.ActionName, apiMethodArguments,
                        actionExecutedContext.Exception.StackTrace);

                    Exception ex = new Exception(consolidatedErrorMessage, actionExecutedContext.Exception);
                    Exception innerMostException = GetInnerMostException(actionExecutedContext.Exception);

                    

                    actionExecutedContext.Response = GetResponseDataForUI(actionExecutedContext.Exception, requestHeader);
                }
            }
            catch (Exception)
            {
               
                actionExecutedContext.Response = GetResponseDataForUI(actionExecutedContext.Exception, actionExecutedContext.ActionContext.Request.Headers);
            }
        }
        #endregion

        #region Private Methods
        private HttpResponseMessage GetResponseDataForUI(Exception ex, HttpRequestHeaders httpRequestHeader)
        {
            string errorContextJson;
            WintrackErrors errorContext = null;
            HttpStatusCode httpStatusCode;

            if (ex is WintrackValidationException)
            {
                WintrackValidationException wintrackException = ex as WintrackValidationException;
                errorContext = new WintrackErrors
                {
                    ErrorCode = wintrackException.ErrorCode,
                    ErrorMessage = wintrackException.Message
                };

                httpStatusCode = HttpErrorResponseHandler.GetHttpErrorResponseCode(wintrackException);
            }
            else
            {
                errorContext = new WintrackErrors
                {
                    ErrorCode = "Unhandled Exception",
                    ErrorMessage = ex.Message
                };

                httpStatusCode = HttpStatusCode.InternalServerError;
            }

            if (httpRequestHeader.Contains("GetFullExceptionWithTrace") &&
                Convert.ToString(httpRequestHeader.GetValues("GetFullExceptionWithTrace").FirstOrDefault()).ToUpper() == "TRUE")
            {
                string fullException = GetFullExceptionWithTrace(ex);
                errorContext.ErrorMessage = errorContext.ErrorMessage + Environment.NewLine + "\"Trace\" : " + fullException;
            }

            errorContextJson = JsonConvert.SerializeObject(errorContext);

            var resp = new HttpResponseMessage(httpStatusCode)
            {
                Content = new StringContent(errorContextJson, Encoding.UTF8, "application/json")
            };

            return resp;
        }

        private string GetFullExceptionWithTrace(Exception ex, int level = 0)
        {
            string exceptionWithTrace;
            StringBuilder sb = new StringBuilder();

            level++;
            if (ex.InnerException != null)
            {
                sb.AppendLine(GetFullExceptionWithTrace(ex.InnerException, level));
            }
            sb.AppendLine("\n------------- Ex Level: " + (level));
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            sb.AppendLine();
            exceptionWithTrace = sb.ToString();
            sb.Clear();
            sb = null;
            return exceptionWithTrace;
        }

        private Exception GetInnerMostException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return GetInnerMostException(ex.InnerException);
            }
            else
            {
                return ex;
            }
        }
        #endregion
    }
}
