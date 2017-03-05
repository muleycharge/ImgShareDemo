namespace ImgShareDemo.Controllers
{
    using ImgShareDemo.BLL;
    using ImgShareDemo.BLL.Exceptions;
    using ImgShareDemo.BLL.Static;
    using ImgShareDemo.BO.Entities;
    using ImgShareDemo.BO.LinkedInResponse;
    using ImgShareDemo.BO.DataTransfer;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Runtime.Caching;
    using System.Collections.Specialized;

    [Authorize]
    public class AccountController : Controller
    {
        private UserService _userService;
        // Use simple caching since this is a demo and doesn't need to scale super large.
        private static Lazy<MemoryCache> _stateCache = new Lazy<MemoryCache>(() => 
        {
            var config = new NameValueCollection();
            // Set really low since this is a demo
            config.Add("CacheMemoryLimitMegabytes", "2");
            return new MemoryCache("LoginStateCache", config);
        });

        private IAuthenticationManager AuthenticationManager
        {
            get { 
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Used by LinkedIn to redirect the user back to our site after they log in.
        /// LinkedIn also uses this for validation so it must match a value
        /// stored in LinkedIn app settings for Authorized Redirect URLs.
        /// </summary>
        private string LocalRedirectUrl
        {
            get
            {
                return Url.Action("LoginLinkedIn", "Account", null, this.Request.Url.Scheme);
            }
        }

        public AccountController()
        {
            _userService = new UserService();
        }

        public AccountController(UserService userService )
        {
            _userService = userService;

        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public async Task<ActionResult> Login(string returnUrl)
        {
            string state = Guid.NewGuid().ToString("N");
            _stateCache.Value.Add(new CacheItem(state, returnUrl ?? "/"), 
                new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
                });
            string loginUrl = Url.Encode(LocalRedirectUrl);
            string linkedInUrl = LinkedInApiService.GetSignInUrl(state, loginUrl);
            return Redirect(linkedInUrl);
        }

        [AllowAnonymous]
        public async Task<ActionResult> LoginLinkedIn(string state, string code)
        {
            if(!String.IsNullOrEmpty(Request.QueryString?["error"]))
            {
                string errorMessage = $"{Request.QueryString?["error"]} {Request.QueryString?["error_description"]}";
                Trace.TraceWarning($"Login failed: Invalid login request from LinkedIn sign in. {errorMessage}");
                return RedirectToAction("ExternalLoginFailure");
            }

            if(String.IsNullOrEmpty(state) || String.IsNullOrEmpty(code))
            {
                Trace.TraceWarning($"Login failed: Improper state or code provided. state: {state}, code: {code}");
                return RedirectToAction("ExternalLoginFailure");
            }
            string returnUrl = _stateCache.Value.Get(state) as string;
            if(returnUrl == null)
            {
                Trace.TraceWarning($"Login failed: Failed to retreive login state from database. state: {state}, code: {code}");
                return RedirectToAction("ExternalLoginFailure");
            }

            try
            {
                IncomingApiDataResponse<LinkedInTokenResponse> tokenResponse = await LinkedInApiService.GetToken(LocalRedirectUrl, code).ConfigureAwait(false);
                LinkedInUser user = await _userService.InitializeUserFromLinkedIn(tokenResponse.Data).ConfigureAwait(false);
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, $"{user.User.FirstName} {user.User.LastName}"),
                    new Claim(ClaimTypes.Email, user.User.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.LinkedInId),
                    new Claim(AppConstants.CustomClaims.IsdUserId, user.Id.ToString())
                };               
                if(!String.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    claims.Add(new Claim(AppConstants.CustomClaims.UserImageUrl, user.ProfileImageUrl));
                } 
                ClaimsIdentity identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                IOwinContext authContext = Request.GetOwinContext();
                authContext.Authentication.SignIn(new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddSeconds(tokenResponse.Data.expires_in) }, identity);
                return RedirectToLocal(returnUrl);
            }
            catch(LinkedInApiResponseException ex)
            {
                Trace.TraceError(ex.ToString(), ex);
                return RedirectToAction("ExternalLoginFailure");
            }
        }
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Logout", "Account");
        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Logout", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userService != null)
                {
                    _userService.Dispose();
                    _userService = null;
                }
            }
            base.Dispose(disposing);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}