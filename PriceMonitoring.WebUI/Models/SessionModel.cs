
using Microsoft.AspNetCore.Http;
using PriceMonitoring.Entities.Concrete;

namespace PriceMonitoring.WebUI.Models
{
    public static class SessionModel
    {
        public static void CreateUserSession(User user, HttpContext httpContext)
        {
            httpContext.Session.SetString(key: "firstName", value: user.FirstName);
            httpContext.Session.SetString(key: "email", value: user.Email);
        }

        public static bool IsSessionExist(string key, HttpContext httpContext)
        {
            return httpContext.Session.GetString(key: key) != null;
        }

        public static void ClearUserSession(HttpContext httpContext)
        {
            httpContext.Session.Clear();
        }
    }
}
