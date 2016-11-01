using BootstrapIntroduction.Models;
using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;   
using System.Web.Mvc.Filters;

namespace BootstrapIntroduction.Filters
{
    public class BasicAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var authorization = request.Headers["Authorization"];

            // No authorization, do nothing
            if (string.IsNullOrEmpty(authorization) || !authorization.Contains("Basic"))
                return;

            // Parse username and password from header
            byte[] encodedDataAsBytes = Convert.FromBase64String(authorization.Replace("Basic ", ""));
            string value = Encoding.ASCII.GetString(encodedDataAsBytes);

            string username = value.Substring(0, value.IndexOf(':'));
            string password = value.Substring(value.IndexOf(':') + 1);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                filterContext.Result = new HttpUnauthorizedResult("Username or password missing");
                return;
            }

            // Validate username and password
            var user = AuthenticatedUsers.Users.FirstOrDefault(u => u.Name == username && u.Password == password);

            if (user == null)
            {
                filterContext.Result = new HttpUnauthorizedResult("Invalid username and password");
                return;
            }

            // Set principal
            filterContext.Principal = new GenericPrincipal(user, user.Roles);
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            filterContext.Result = new BasicChallengeActionResult
            {
                CurrrentResult = filterContext.Result
            };
        }
    }

    class BasicChallengeActionResult : ActionResult
    {
        public ActionResult CurrrentResult { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            CurrrentResult.ExecuteResult(context);

            var response = context.HttpContext.Response;

            if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
                response.AddHeader("WWW-Authenticate", "Basic");
        }
    }
}