namespace ImgShareDemo.BLL
{
    using ImgShareDemo.BLL.Static;
    using System;
    using System.Security.Claims;
    using System.Security.Principal;

    public static class IdentityExtensions
    {
        public static string GetUserImageUrl(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            if (ci != null)
            {
                return ci.FindFirstValue(AppConstants.CustomClaims.UserImageUrl);
            }
            return null;
        }

        public static int? GetUserId(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            if (ci != null)
            {
                int userId;
                if(int.TryParse(ci.FindFirstValue(AppConstants.CustomClaims.IsdUserId), out userId))
                {
                    return userId;
                }
            }
            return null;
        }

        /// <summary>
        ///     Return the claim value for the first claim with the specified type if it exists, null otherwise
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var claim = identity.FindFirst(claimType);
            return claim != null ? claim.Value : null;
        }
    }
}
