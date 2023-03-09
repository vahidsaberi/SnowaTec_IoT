using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SnowaTec.Test.Web.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Request.Cookies.Count > 0)
            {
                foreach (var cookieKey in Request.Cookies.Keys)
                {
                    HttpContext.Response.Cookies.Delete(cookieKey);
                }
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
