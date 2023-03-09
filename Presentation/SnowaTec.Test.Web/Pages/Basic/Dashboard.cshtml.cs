using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SnowaTec.Test.Web.Pages.Basic
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
