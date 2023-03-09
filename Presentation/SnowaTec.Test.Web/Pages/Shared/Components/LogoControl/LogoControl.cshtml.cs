using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Interfaces.Base;

namespace SnowaTec.Test.Web.Pages.Shared.Components.LogoControl
{
    [ViewComponent(Name = "LogoControl")]
    public class LogoControl : ViewComponent
    {
        [Inject]
        public ICallApiService _service { get; }

        public LogoControl(ICallApiService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _service.Call<PortalInfo>("", ApiRoutes.PortalInfos.Get, CallMethodType.Get, null, ("Id", 1));

            return View("LogoControl", result.Data);
        }
    }
}
