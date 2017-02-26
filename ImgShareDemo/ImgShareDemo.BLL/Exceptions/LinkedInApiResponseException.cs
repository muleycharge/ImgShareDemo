using ImgShareDemo.BO.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace ImgShareDemo.BLL.Exceptions
{
    public class LinkedInApiResponseException : Exception
    {
        public HttpResponseMessage Response { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public LinkedInErrorResponse LinkedInError { get; private set; }

        public LinkedInApiResponseException(string message, HttpRequestMessage request, HttpResponseMessage response) : base(message)
        {
            Request = request;
            Response = response;
            // Attempt to get additional error information from response, but
            // we don't want to throw another exception if we fail.
            try
            {
                string body = Response.Content.ReadAsStringAsync().Result;
                LinkedInError = JsonConvert.DeserializeObject<LinkedInErrorResponse>(body);
            }
            catch (Exception)
            {
                LinkedInError = new LinkedInErrorResponse
                {
                    errorCode = -1,
                    message = "Unable to get additional information from error message",
                    status = (int)Response.StatusCode,
                    timestamp = 0
                };
            }
        }

        public override string ToString()
        {            
            return $"Failed call to LinkedIn Api{Environment.NewLine}{LinkedInError.message}{Environment.NewLine}{ base.ToString()}";
        }
    }
}
