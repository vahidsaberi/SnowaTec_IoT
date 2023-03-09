using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using SnowaTec.Test.Domain.Enum;

namespace SnowaTec.Test.Web.Pages.Shared.Components.BreadcrumbControl
{
    [ViewComponent(Name = "BreadcrumbControl")]
    public class BreadcrumbControl : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty]
        public Dictionary<string, string> UrlItems { get; set; }

        public BreadcrumbControl(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            UrlItems = new Dictionary<string, string>();
        }

        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var URL = _httpContextAccessor.HttpContext.Request.GetDisplayUrl();
            var schema = request.Scheme;
            var host = request.Host;
            var route = URL.Replace($"{schema}://{host}", "");

            var home = string.Empty;

            if(User.IsInRole(Roles.SuperAdmin.ToString()))
            {
                home = "/SuperAdmin/Dashboard";
            }
            else if (User.IsInRole(Roles.Admin.ToString()))
            {
                home = "/Admin/Dashboard";
            }
            else if (User.IsInRole(Roles.Moderator.ToString()))
            {
                home = "/Moderator/Dashboard";
            }
            else if (User.IsInRole(Roles.Basic.ToString()))
            {
                home = "/Basic/Dashboard";
            }
            else
            {
                home = "/Index";
            }

            route = route.Replace(home,"");

            UrlItems.Add("صفحه اصلی", home);

            var path = string.Empty;

            route.Split('/').ToList().ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x))
                {
                    path += $"/{x}";

                    UrlItems.Add(x, path);
                }
            });


            return View("BreadcrumbControl", UrlItems);
        }
    }
}
