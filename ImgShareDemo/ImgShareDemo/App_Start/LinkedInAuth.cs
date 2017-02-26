using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Logging;
using System.Net.Http;
using ImgShareDemo.BO.DataTransfer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ImgShareDemo.App_Start
{
    public class LinkedInAuthHandler : AuthenticationHandler<LinkedInAuthenticationOptions>
    {
        private readonly ILogger _logger;
        public LinkedInAuthHandler(ILogger logger)
        {
            this._logger = logger;
        }
        protected async override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            try
            {
                AuthenticationProperties properties = null;
                string code = null;
                string state = null;
                IReadableStringCollection query = Request.Query;
                IList<string> values = query.GetValues("error");
                if (values != null && values.Count >= 1)
                {
                    _logger.WriteVerbose("Remote server returned an error: " + Request.QueryString);
                }

                values = query.GetValues("code");
                if (values != null && values.Count == 1)
                {
                    code = values[0];
                }
                values = query.GetValues("state");
                if (values != null && values.Count == 1)
                {
                    state = values[0];
                }

                HttpClient client = new HttpClient();
                HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type",  "authorization_code" },
                    { "code",  code },
                    { "redirect_uri",  Options.RedirectUrl },
                    { "client_id",  Options.ClientId },
                    { "client_secret",  Options.ClientSecret }
                });
                //string linkedInUrl = "https://www.linkedin.com/oauth/v2/accessToken";
                HttpResponseMessage response = await client.PostAsync(Options.SignInEndpoint, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    LinkedInTokenResponse tokenResponse = await response.Content.ReadAsAsync<LinkedInTokenResponse>().ConfigureAwait(false);
                    SecurityToken token = new JwtSecurityToken();
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    token = tokenHandler.ReadToken(tokenResponse.access_token);
                    JwtSecurityToken jwtToken = (JwtSecurityToken)token;
                    ClaimsIdentity identity = new ClaimsIdentity(jwtToken.Claims, Options.AuthenticationType);
                    Context.Authentication.SignIn();
                }
                else
                {
                    throw new NotImplementedException();
                }

            }
            catch(Exception)
            {
                throw;
            }
            throw new NotImplementedException();
        }
    }
    public class LinkedInAuthenticationOptions : AuthenticationOptions
    {
        public const string AuthenticationTypeName = "LinkedIn";

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string RedirectUrl { get; set; }

        public string SignInEndpoint { get; set; }

        public LinkedInAuthenticationOptions()
             : base(AuthenticationTypeName)
        {
        }
        
    }
    public class LinkedInAuthenticationMiddleware : Microsoft.Owin.Security.Infrastructure.AuthenticationMiddleware<LinkedInAuthenticationOptions>
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public LinkedInAuthenticationMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            LinkedInAuthenticationOptions options) : base(next, options)
        {
            _logger = app.CreateLogger<LinkedInAuthHandler>();

            if (string.IsNullOrWhiteSpace(Options.ClientId))
            {
                throw new ArgumentException($"{nameof(LinkedInAuthenticationOptions)} is missing required configuration option for {nameof(Options.ClientId)}.");
            }
            if (string.IsNullOrWhiteSpace(Options.ClientSecret))
            {
                throw new ArgumentException($"{nameof(LinkedInAuthenticationOptions)} is missing required configuration option for {nameof(Options.ClientSecret)}.");
            }
            if (string.IsNullOrWhiteSpace(Options.RedirectUrl))
            {
                throw new ArgumentException($"{nameof(LinkedInAuthenticationOptions)} is missing required configuration option for {nameof(Options.RedirectUrl)}.");
            }
            if (string.IsNullOrWhiteSpace(Options.SignInEndpoint))
            {
                throw new ArgumentException($"{nameof(LinkedInAuthenticationOptions)} is missing required configuration option for {nameof(Options.SignInEndpoint)}.");
            }
        }

        protected override AuthenticationHandler<LinkedInAuthenticationOptions> CreateHandler()
        {
            return new LinkedInAuthHandler(_logger);
        }
    }
}