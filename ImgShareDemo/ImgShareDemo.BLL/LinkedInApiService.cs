namespace ImgShareDemo.BLL
{
    using BO.LinkedInResponse;
    using ImgShareDemo.BLL.Exceptions;
    using ImgShareDemo.BLL.Static;
    using ImgShareDemo.BO.DataTransfer;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class LinkedInApiService
    {
        #region Fields
        private HttpClient _client;
        #endregion

        #region Constructors
        public LinkedInApiService(string token)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(AppConfig.LinkedInBaseUrl.Value);

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("x-li-format", "json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        #endregion

        #region Methods
        public async Task<IncomingApiDataResponse<LinkedInPersonResponse>> GetPerson(string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "v1/people/~:(id,first-name,last-name,headline,picture-url,public-profile-url,email-address)?format=json");

            HttpResponseMessage response = await _client.SendAsync(request)
                .ContinueWith(responseTask =>
                {
                    Trace.TraceInformation($"Get Person Response: {responseTask.Result}");
                    return responseTask.Result;
                }).ConfigureAwait(false);
            if(!response.IsSuccessStatusCode)
            {
                throw new LinkedInApiResponseException($"Call to LinkedIn API failed server returned {response.StatusCode} - {response.ReasonPhrase}", request, response);
            }
            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new IncomingApiDataResponse<LinkedInPersonResponse>
            {
                Response = response,
                Data = JsonConvert.DeserializeObject<LinkedInPersonResponse>(responseContent)
            };
        }
        #endregion

        #region Static Declaration
        public static string GetSignInUrl(string state, string loginUrl)
        {
            string linkedInUrl = $"{AppConfig.LinkedInBaseUrl.Value}/oauth/v2/authorization?response_type=code&state={state}&client_id={AppConfig.LinkedInClientId.Value}&scope=r_basicprofile,r_emailaddress&redirect_uri={loginUrl}";
            return linkedInUrl;
        }
        public static async Task<IncomingApiDataResponse<LinkedInTokenResponse>> GetToken(string redirectUrl, string code)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(AppConfig.LinkedInBaseUrl.Value);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/oauth/v2/accessToken");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type",  "authorization_code" },
                    { "code",  code },
                    { "redirect_uri",  redirectUrl },
                    { "client_id",  AppConfig.LinkedInClientId.Value },
                    { "client_secret",  AppConfig.LinkedInClientSecret.Value }
                });
            HttpResponseMessage response = await client.SendAsync(request)
                .ContinueWith(responseTask =>
                {
                    Trace.TraceInformation($"Get Token Response: {responseTask.Result}");
                    return responseTask.Result;
                }).ConfigureAwait(false);
            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new LinkedInApiResponseException($"Call to LinkedIn API failed server returned {response.StatusCode} - {response.ReasonPhrase}", request, response);
            }

            return new IncomingApiDataResponse<LinkedInTokenResponse>
            {
                Response = response,
                Data = JsonConvert.DeserializeObject<LinkedInTokenResponse>(responseContent)
            };
        }
        #endregion
    }
}
