using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Web.Contract;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Interfaces.Base;

namespace SnowaTec.Test.Web.Pages.Shared.Components.MenuControl
{
    [ViewComponent(Name = "MenuControl")]
    public class MenuControl : ViewComponent
    {
        [Inject]
        public ICallApiService _service { get; }

        [Inject]
        public IPermission _permission { get; }

        [Inject]
        public IHttpContextAccessor _httpContextAccessor { get; }

        public MenuControl(ICallApiService service,
            IPermission permission,
            IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _permission = permission;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (Request.Cookies["AccessToken"] == null)
                return new HtmlContentViewComponentResult(new HtmlString("<b>Not load</b>"));

            var token = Request.Cookies["AccessToken"].ToString();

            var SystemPart = await _service.Call<List<SystemPart>>(token, ApiRoutes.SystemParts.GetAll, CallMethodType.Get);

            if (!User.IsInRole(Roles.SuperAdmin.ToString()) && SystemPart.Data.Count() > 0)
            {
                var allPermission = await _permission.GetAllAccesses();

                var ids = allPermission.Select(x => x.Id).ToList();

                SystemPart.Data.RemoveAll(x => !ids.Contains(x.Id));
            }

            var menus = ParentChildModelConvert.RecursiceContent(SystemPart.Data, null, "Id", "ParentId", "SubSystemParts");

            if (!User.IsInRole(Roles.SuperAdmin.ToString()) && menus.Count() > 0)
            {
                menus.Remove(menus.FirstOrDefault(x => x.EnglishTitle == Roles.SuperAdmin.ToString()));
            }

            return View("MenuControl", menus);
        }
    }
}
