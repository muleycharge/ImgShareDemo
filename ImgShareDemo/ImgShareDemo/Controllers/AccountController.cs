namespace ImgShareDemo.Controllers
{
    using BLL;
    using BLL.Exceptions;
    using BLL.Static;
    using BO.Entities;
    using ImgShareDemo.BO.DataTransfer;
    using ImgShareDemo.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    [Authorize]
    public class AccountController : Controller
    {
        private UserService _userService;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
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
            UserSignOn uso = await _userService.CreateSignOnState(returnUrl).ConfigureAwait(false);
            string loginUrl = Url.Encode(LocalRedirectUrl);
            string linkedInUrl = LinkedInApiService.GetSignInUrl(uso.State, loginUrl);
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

            UserSignOn uso = await _userService.GetSignOnState(state).ConfigureAwait(false);
            if(uso == null)
            {
                Trace.TraceWarning($"Login failed: Failed to retreive login state from database. state: {state}, code: {code}");
                return RedirectToAction("ExternalLoginFailure");
            }

            if(uso.State != state)
            {
                Trace.TraceWarning($"Login failed: Failed to validate CSFR state. state: {state}, code: {code}");
                return RedirectToAction("ExternalLoginFailure");
            }
            try
            {
                ApiDataResponse<LinkedInTokenResponse> tokenResponse = await LinkedInApiService.GetToken(LocalRedirectUrl, code).ConfigureAwait(false);
                LinkedInUser user = await _userService.InitializeUserFromLinkedIn(tokenResponse.Data).ConfigureAwait(false);
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, $"{user.User.FirstName} {user.User.LastName}"),
                    new Claim(ClaimTypes.Email, user.User.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.LinkedInId)
                };                
                ClaimsIdentity identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                IOwinContext authContext = Request.GetOwinContext();
                authContext.Authentication.SignIn(new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddSeconds(tokenResponse.Data.expires_in) }, identity);
                return RedirectToLocal(uso.RedirectUri);
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
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
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