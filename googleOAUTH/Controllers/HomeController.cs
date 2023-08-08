using System.Diagnostics;
using System.Text;
using Google.Apis.Oauth2.v2;
using Microsoft.AspNetCore.Mvc;
using googleOAUTH.Models;

namespace googleOAUTH.Controllers;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Authenticate(string email) 
        {
            return RedirectPermanent(GoogleApiHelper.GetOauthUri(email));
        }

        public IActionResult OauthCallback(string code,string error, string state)
        {
            try
            {
                if (!string.IsNullOrEmpty(code)) 
                {
                    ViewBag.Message = "Successfull: " + code;
                }
                if (!string.IsNullOrEmpty(error)) 
                {
                    ViewBag.Message = "Error: " + error;
                }
                if (!string.IsNullOrEmpty(state)) 
                {
                    ViewBag.MailAddress = state;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

public class GoogleApiHelper
{
    public static string ApplicationName = "Google Api DotNetCore Web Client";

    public static string ClientId = "596025371422-b9ajo9hct5mhqlki9jdvkc5sqllh93vd.apps.googleusercontent.com";

    public static string ClientSecret = "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";

    public static string RedirectUri = "http://localhost:8081/Home/OauthCallback";

    public static string OauthUri = "https://accounts.google.com/o/oauth2/auth?";

    public static string Scopes = "https://www.googleapis.com/auth/userinfo.email";

    public static string GetOauthUri(string extraParam) 
    {
        StringBuilder sbUri = new StringBuilder(OauthUri);
        sbUri.Append("client_id=" + ClientId);
        sbUri.Append("&redirect_uri=" + RedirectUri);
        sbUri.Append("&response_type=" + "code");
        sbUri.Append("&scope=" + Scopes);
        sbUri.Append("&access_type=" + "offline");
        sbUri.Append("&state=" + extraParam);

        return sbUri.ToString();
    }
}