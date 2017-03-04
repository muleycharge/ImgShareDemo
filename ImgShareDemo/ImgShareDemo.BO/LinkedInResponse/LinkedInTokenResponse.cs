namespace ImgShareDemo.BO.LinkedInResponse
{
    public class LinkedInTokenResponse
    {
        /// <summary>
        /// The access token for the user.  This value must be kept secure, as per your agreement to the API Terms of Use.
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// The number of seconds remaining, from the time it was requested, before the token will expire.  Currently, all access tokens are issued with a 60 day lifespan.
        /// </summary>
        public int expires_in { get; set; }
    }
}
