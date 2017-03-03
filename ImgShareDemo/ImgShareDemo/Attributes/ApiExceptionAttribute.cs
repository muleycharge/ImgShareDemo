using ImgShareDemo.BLL.Exceptions;
using ImgShareDemo.BLL.Static;
using ImgShareDemo.BO.DataTransfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace ImgShareDemo.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            // Service level exceptions contain messages that are displayable to the user
            ServiceLevelException serviceEx = GetException<ServiceLevelException>(actionExecutedContext.Exception);
            ApiResponse apiResponse = new ApiResponse();
            if (serviceEx != null)
            {
                string errorKey = String.IsNullOrEmpty(serviceEx.ErrorKey) ? "Service Error" : serviceEx.ErrorKey;
                apiResponse.Errors.Add(new KeyValuePair<string, string>("Service Error", serviceEx.Message));
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, apiResponse, "application/json");
                return;
            }

            // Catch all operation for creating response for messages
            apiResponse.Errors.Add(new KeyValuePair<string, string>("Error", "There was an error while trying to process your request."));
            if(ConfigurationManager.AppSettings["Environment"] == "Development")
            {
                apiResponse.Data = Newtonsoft.Json.JsonConvert.SerializeObject(actionExecutedContext.Exception);
            }

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, apiResponse, "application/json");
            return;
        }

        private T GetException<T>(Exception ex) where T : Exception
        {
            if (ex is T || (ex is AggregateException && (ex as AggregateException).InnerExceptions.Any(e => e is T)))
            {
                return ex as T ?? (ex as AggregateException).InnerExceptions.First(e => e is T) as T;
            }
            return null;
        }
    }
}