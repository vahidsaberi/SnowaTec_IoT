using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Web.Contract;
using SnowaTec.Test.Web.Extensions;
using SnowaTec.Test.Web.Interfaces.Base;

namespace SnowaTec.Test.Web.Pages.SuperAdmin.SystemPart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public ICallApiService _service { get; }

        public Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment { get; }

        [BindProperty]
        public Domain.Entities.Security.SystemPart Input { get; set; }

        public IndexModel(ICallApiService service, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        #region Methods
        [NonAction]
        private List<string> LoadDirectores(string path)
        {
            var folders = new List<string>();

            string[] paths = Directory.GetDirectories(Path.Combine(this._environment.WebRootPath, path));

            foreach (string item in paths)
            {
                var dirName = new DirectoryInfo(item).Name;

                if (dirName == "Shared")
                    continue;

                folders.Add(dirName);
                folders.AddRange(LoadDirectores($"{path}/{dirName}"));
            }

            return folders.Distinct().OrderBy(x => x).ToList();
        }
        #endregion

        #region Base Data
        public async Task<JsonResult> OnGetAllDirectoresAsync(string path, long parentId)
        {
            var a = this.LoadDirectores(path);
            var data = new Dictionary<string, string>();
            if (parentId == 0)
            {
                data.Add("مدیریت پرتال","SuperAdmin");
                data.Add("مدیریت","Admin");
                data.Add("لوازم هوشمند", "SmartAppliances");
                data.Add("پایه","Admin");
            }
            return new JsonResult(data);
        }

        public async Task<JsonResult> OnGetAllPermissionTypesAsync()
        {
            var permissions = PermissionTypeFunction.ToSelectedList();

            return new JsonResult(permissions);
        }

        public async Task<JsonResult> OnGetAllMenuTypesAsync()
        {
            var SystemPartTypes = MenuTypeFunction.ToSelectedList();

            return new JsonResult(SystemPartTypes);
        }
        #endregion

        public async Task<JsonResult> OnGetCreateUpdateAsync(int? id, long? parentId)
        {
            var model = new Domain.Entities.Security.SystemPart
            {
                ParentId = parentId,
                MenuType = MenuType.Menu
            };

            var token = Request.Cookies["AccessToken"].ToString();

            if (id != null)
            {
                var result = await _service.Call<Domain.Entities.Security.SystemPart>(token, ApiRoutes.SystemParts.Get, Enums.CallMethodType.Get, model: null, ("id", id));
                model = result.Data;
            }

            if (parentId != null)
            {
                var parent = await _service.Call<Domain.Entities.Security.SystemPart>(token, ApiRoutes.SystemParts.Get, Enums.CallMethodType.Get, model: null, ("id", parentId));
                model.ParentName = parent.Data.PersianTitle;
            }

            return new JsonResult(model);
        }

        public async Task<PartialViewResult> OnGetSystemPartsAsync()
        {
            var systemPart = await _service.Call<List<Domain.Entities.Security.SystemPart>>(Request.Cookies["AccessToken"].ToString(), ApiRoutes.SystemParts.GetAll, Enums.CallMethodType.Get);

            var model = new List<Domain.Entities.Security.SystemPart>();

            if (systemPart.Succeeded && systemPart.Data.Count > 0)
            {
                model = ParentChildModelConvert.RecursiceContent(systemPart.Data, null, "Id", "ParentId", "SubSystemParts");
            }

            return LoadPartialView.Show<List<Domain.Entities.Security.SystemPart>>("_Tree", model, ViewData, TempData);
        }

        public async Task<PartialViewResult> OnGetSystemPartAsync(long id)
        {
            var SystemPart = await _service.Call<Domain.Entities.Security.SystemPart>(Request.Cookies["AccessToken"].ToString(), ApiRoutes.SystemParts.Get, Enums.CallMethodType.Get, model: null, ("id", id));

            return LoadPartialView.Show<Domain.Entities.Security.SystemPart>("_Info", SystemPart.Data, ViewData, TempData);
        }

        public async Task<JsonResult> OnPostSaveSystemPartAsync(Domain.Entities.Security.SystemPart data)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            if (data == null)
            {
                return new JsonResult(new Response<Domain.Entities.Security.SystemPart>
                {
                    Succeeded = false,
                    Errors = new List<string> { "اطلاعات ارسال شده اشتباه است." }
                });
            }

            if (string.IsNullOrWhiteSpace(data.PersianTitle) || string.IsNullOrWhiteSpace(data.EnglishTitle))
            {
                return new JsonResult(new Response<Domain.Entities.Security.SystemPart>
                {
                    Succeeded = false,
                    Errors = new List<string> { "اطلاعات ارسال شده اشتباه است." }
                });
            }

            var all = await _service.Call<List<Domain.Entities.Security.SystemPart>>(token, ApiRoutes.SystemParts.GetAll, Enums.CallMethodType.Get);

            if (data.ParentId == null)
            {
                if (data.MenuType == MenuType.License)
                {
                    return new JsonResult(new Response<Domain.Entities.Security.SystemPart>
                    {
                        Succeeded = false,
                        Errors = new List<string> { "برای شاخه اصلی نمی توانید دسترسی تعریف کنید." }
                    });
                }
            }
            else
            {
                var find = all.Data.SingleOrDefault(x => x.Id == data.ParentId);

                if (find.MenuType == MenuType.Group && data.MenuType == MenuType.License)
                {
                    return new JsonResult(new Response<Domain.Entities.Security.SystemPart>
                    {
                        Succeeded = false,
                        Errors = new List<string> { "برای گروه منو نمی توانید دسترسی تعریف کنید." }
                    });
                }
            }

            if (data.Id > 0)
            {
                var previousData = all.Data.FirstOrDefault(x => x.Id == data.Id);

                if (data.ParentId == null)
                {
                    if (previousData.EnglishTitle != data.EnglishTitle && all.Data.Any(x => x.ParentId == null && x.EnglishTitle == data.EnglishTitle))
                    {
                        return new JsonResult(new Response<Domain.Entities.Security.SystemPart>
                        {
                            Succeeded = false,
                            Errors = new List<string> { "نام لاتین وارد شده تکراری است." }
                        });
                    }
                }

                if (data.ParentId != null)
                {
                    if (previousData.EnglishTitle != data.EnglishTitle && all.Data.Any(x => x.ParentId == data.ParentId && x.EnglishTitle == data.EnglishTitle))
                    {
                        return new JsonResult(new Response<Domain.Entities.Security.SystemPart>
                        {
                            Succeeded = false,
                            Errors = new List<string> { "نام لاتین وارد شده تکراری است." }
                        });
                    }
                }
            }
            else
            {
                if (data.ParentId == null)
                {
                    if (all.Data.Any(x => x.ParentId == null && x.EnglishTitle == data.EnglishTitle))
                    {
                        return new JsonResult(new Domain.Common.Response<Domain.Entities.Security.SystemPart>
                        {
                            Succeeded = false,
                            Errors = new List<string> { "نام لاتین وارد شده تکراری است." }
                        });
                    }
                }

                if (data.ParentId != null)
                {
                    if (all.Data.Any(x => x.ParentId == data.ParentId && x.EnglishTitle == data.EnglishTitle))
                    {
                        return new JsonResult(new Response<Domain.Entities.Security.SystemPart>
                        {
                            Succeeded = false,
                            Errors = new List<string> { "نام لاتین وارد شده تکراری است." }
                        });
                    }
                }
            }

            var result = new Response<Domain.Entities.Security.SystemPart>();

            var model = new Domain.Entities.Security.SystemPart
            {
                Id = data.Id,
                ParentId = data.ParentId,
                PersianTitle = data.PersianTitle,
                EnglishTitle = data.EnglishTitle,
                MenuType = data.MenuType,
                IconName = data.IconName,
                AdditionalPermissions = data.AdditionalPermissions
            };

            if (data.Id == 0)
            {
                result = await _service.Call<Domain.Entities.Security.SystemPart>(token, ApiRoutes.SystemParts.Create, Enums.CallMethodType.Post, model);
            }
            else
            {
                result = await _service.Call<Domain.Entities.Security.SystemPart>(token, ApiRoutes.SystemParts.Update, Enums.CallMethodType.Put, model, ("id", data.Id));
            }

            return new JsonResult(result);
        }

        public async Task<JsonResult> OnPostDeleteSystemPartAsync(long id)
        {
            var result = await _service.Call<Domain.Entities.Security.SystemPart>(Request.Cookies["AccessToken"].ToString(), ApiRoutes.SystemParts.Delete, Enums.CallMethodType.Delete, model: null, ("id", id), ("type", 3));

            return new JsonResult(result);
        }
    }
}
