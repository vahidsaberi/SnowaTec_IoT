using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Web.Interfaces.Base;

namespace SnowaTec.Test.Web.Pages.SuperAdmin
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly ICallApiService _service;

        public DashboardModel(ICallApiService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> OnGetActiveUserAsync()
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                Expression<Func<ApplicationUser, bool>> expected = x => x.LockoutEnabled == false;
                var users = await _service.Call<List<ApplicationUser>>(token, ApiRoutes.Users.Filter, Enums.CallMethodType.Post, expected);

                return new JsonResult(users.Data);
            }
            catch //(Exception ex)
            {

            }

            return null;
        }
    }
}
