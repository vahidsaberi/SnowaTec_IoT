using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;

namespace SnowaTec.Test.Web.Pages.SuperAdmin.Installer
{
    [Authorize]
    public class IndexModel : PageModel
    {
        [Inject]
        public ICallApiService _service { get; }

        [Inject]
        private IMapper _mapper { get; }

        [Inject]
        public Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment { get; }

        public IndexModel(ICallApiService service, IMapper mapper, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _service = service;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<JsonResult> OnPostAsync(DTParameters dtModel)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var result = await GetPlugins();

            return new JsonResult(new DTResult<Domain.Entities.Portal.Plugin>
            {
                data = result,
                draw = result.Count,
                recordsFiltered = 0,
                recordsTotal = result.Count
            });
        }

        public async Task<List<Domain.Entities.Portal.Plugin>> GetPlugins()
        {
            var basePath = new List<string> { "Shared", "Authentication", "SuperAdmin", "Admin", "Moderator", "Basic" };
            var path = "Pages";

            string[] paths = Directory.GetDirectories(Path.Combine(this._environment.ContentRootPath, path));

            var plugins = paths.Where(x => !basePath.Contains(new DirectoryInfo(x).Name)).Select(x => new Domain.Entities.Portal.Plugin { Name = new DirectoryInfo(x).Name }).ToList();

            var token = Request.Cookies["AccessToken"].ToString();

            var menus = plugins.Select(x => x.Name).ToList();
            Expression<Func<Domain.Entities.Security.SystemPart, bool>> expression = x => menus.Contains(x.EnglishTitle);
            var systemParts = await _service.Call<List<Domain.Entities.Security.SystemPart>>(token, ApiRoutes.SystemParts.Filter, Enums.CallMethodType.Post, expression);

            if (systemParts.Data.Count > 0)
            {
                systemParts.Data.ForEach(x =>
                {
                    var find = plugins.FirstOrDefault(y => y.Name == x.EnglishTitle);

                    find.Installed = true;
                });
            }

            return plugins;
        }

        public async Task<ActionResult> OnPostInstallAsync(string pluginName)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var basePath = $"{AppDomain.CurrentDomain.FriendlyName}.Pages";

                var menus = typeof(Program).Assembly.ExportedTypes.Where(x =>
                    typeof(IMenu).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(Activator.CreateInstance).Cast<IMenu>().ToList();

                var menu = menus.FirstOrDefault(x => x.ToString() == $"{basePath}.{pluginName}.Menus");

                var items = menu.GetAll();

                var result = new List<Response<Domain.Entities.Security.SystemPart>>();
                foreach (var item in items)
                {
                    if (item.Parent != null)
                    {
                        var find = result.FirstOrDefault(x => x.Data.EnglishTitle == item.Parent.EnglishTitle);
                        item.Parent = null;
                        item.ParentId = find.Data.Id;
                    }

                    var answer = await _service.Call<Domain.Entities.Security.SystemPart>(token, ApiRoutes.SystemParts.Create, Enums.CallMethodType.Post, item);

                    result.Add(answer);
                }

                return new JsonResult(new Response<bool>(true, "نصب پلاگین با موفقیت انجام شد."));

            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<bool> { Message = "خطا در نصب پلاگین", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<ActionResult> OnPostUninstallAsync(string pluginName)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                Expression<Func<Domain.Entities.Security.SystemPart, bool>> expression = x => x.EnglishTitle == pluginName;
                var systemParts = await _service.Call<List<Domain.Entities.Security.SystemPart>>(token, ApiRoutes.SystemParts.Filter, Enums.CallMethodType.Post, expression);

                var savedMenus = new List<Domain.Entities.Security.SystemPart>();

                while (systemParts.Data.Count > 0)
                {
                    var finded = systemParts.Data;
                    savedMenus.AddRange(finded);

                    var parenIds = finded.Select(x => x.Id).ToList();

                    expression = x => parenIds.Contains(x.ParentId ?? 0);
                    systemParts = await _service.Call<List<Domain.Entities.Security.SystemPart>>(token, ApiRoutes.SystemParts.Filter, Enums.CallMethodType.Post, expression);
                }

                var result = new List<Response<Domain.Entities.Security.SystemPart>>();
                foreach (var item in savedMenus.OrderByDescending(x => x.Id).ToList())
                {
                    result.Add(await _service.Call<Domain.Entities.Security.SystemPart>(token, ApiRoutes.SystemParts.Delete, Enums.CallMethodType.Delete, null, ("Id", item.Id), ("Type", 4)));
                }

                return new JsonResult(new Response<bool>(true, "حذف پلاگین با موفقیت انجام شد."));

            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<bool> { Message = "خطا در حذف پلاگین", Errors = new List<string> { ex.Message } });
            }
        }
    }
}
