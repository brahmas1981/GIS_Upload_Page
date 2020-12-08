using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GIS_Upload_Page.Extensions
{
    public static class UserManagerExtensions
    {
        public static string GetCurrentUser(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string GetLoggedInUserName(this ClaimsPrincipal user)
        {
            var loggedInName = user.FindFirst("name")?.Value;
            if (string.IsNullOrEmpty(loggedInName))
            {
                var firstName = user.FindFirst(ClaimTypes.GivenName)?.Value;
                var lastName = user.FindFirst(ClaimTypes.Surname)?.Value;
                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                    loggedInName = firstName + " " + lastName;
                else
                    loggedInName = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            return loggedInName;
        }

        public static dynamic GetLoggedInUserInfo(this ClaimsPrincipal user)
        {
            var userName = string.Empty;

            if (string.IsNullOrEmpty(userName))
            {
                userName = user.GetCurrentUser();
                if (string.IsNullOrEmpty(userName))
                    userName = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            var fullName = user.FindFirst("name")?.Value;

            var userEmail = user.FindFirst("email")?.Value;

            if (!userEmail.IsValidEmail())
            {
                // try email claim type
                userEmail = user.FindFirst(ClaimTypes.Email)?.Value;
                if (!userEmail.IsValidEmail())
                {
                    // try upn
                    userEmail = user.FindFirst(ClaimTypes.Upn)?.Value;
                    // try name
                    if (!userEmail.IsValidEmail())
                        userEmail = user.FindFirst(ClaimTypes.Name)?.Value;
                }
            }

            return new { UserName = userName, FullName = fullName, UserEmail = userEmail };
        }
    }
}
