using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Web.Extensions;
using SnowaTec.Test.Web.Interfaces.Base;

namespace SnowaTec.Test.Web.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        [Inject]
        private ICallApiService _service { get; }

        [Inject]
        private IMapper _mapper { get; }

        public IndexModel(ICallApiService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetStatusAsync()
        {
            var token = Request.Cookies["AccessToken"].ToString();
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var userRoles = await _service.Call<List<string>>(token, ApiRoutes.Accounts.GetUserRoles, Enums.CallMethodType.Get, model: null, ("userId", userId));

            
            return LoadPartialView.Show<object>("_Status", null, ViewData, TempData);
        }
    }
}