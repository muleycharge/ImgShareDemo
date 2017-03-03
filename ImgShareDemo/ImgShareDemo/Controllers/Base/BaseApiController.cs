namespace ImgShareDemo.Controllers.Base
{
    using BLL;
    using BLL.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    public class BaseApiController : ApiController
    {
        protected int UserId
        {
            get
            {
                int? userId = User.Identity.GetUserId();
                if (userId.HasValue)
                {
                    return userId.Value;
                }
                else
                {
                    throw new InvalidOperationException("User context is not set. Unable to get user ID.");
                }
            }
        }
    }
}