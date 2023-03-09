using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Possibility;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;
using System.Security.Claims;

namespace SnowaTec.Test.Web.Pages.SuperAdmin.PortalInfo
{
    [Authorize]
    public class IndexModel : PageModel
    {
        [Inject]
        public ICallApiService _service { get; }

        [Inject]
        private IMapper _mapper { get; }

        [BindProperty]
        public PortalInfoDto PortalInfo { get; set; }

        [TempData]
        public string Message { get; set; }

        public string ErrorMessage { get; set; }

        public IndexModel(ICallApiService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGet()
        {
            await this.GetLastInfo();

            return Page();
        }

        private async Task GetLastInfo()
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var result = await _service.Call<Domain.Entities.Possibility.PortalInfo>("", ApiRoutes.PortalInfos.Get, CallMethodType.Get, model: null, ("Id", 1));

            this.PortalInfo = _mapper.Map<PortalInfoDto>(result.Data);

            var keys = new List<string> { "Logo", "FavIcon", "SmallLogo", "DeveloperLogo" };
            Expression<Func<Document, bool>> expected = x => x.ModuleId == ModuleType.PortalInfo && keys.Contains(x.Key);
            var documents = await _service.Call<List<Document>>(token, ApiRoutes.Documents.Filter, CallMethodType.Post, expected);

            if (documents.Data.Count > 0)
            {
                var logo = documents.Data.FirstOrDefault(x => x.Key == "Logo");
                if (logo != null)
                    PortalInfo.Logo = logo.Content ?? null;

                var favIcon = documents.Data.FirstOrDefault(x => x.Key == "FavIcon");
                if (favIcon != null)
                    PortalInfo.FavIcon = favIcon.Content ?? null;

                var smallLogo = documents.Data.FirstOrDefault(x => x.Key == "SmallLogo");
                if (smallLogo != null)
                    PortalInfo.SmallLogo = smallLogo.Content ?? null;

                var developerLogo = documents.Data.FirstOrDefault(x => x.Key == "DeveloperLogo");
                if (developerLogo != null)
                    PortalInfo.DeveloperLogo = developerLogo.Content ?? null;
            }
        }

        public async Task<IActionResult> OnPost()
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var keys = new List<string> { "Logo", "FavIcon", "SmallLogo", "DeveloperLogo" };
            Expression<Func<Document, bool>> expected = x => x.ModuleId == ModuleType.PortalInfo && keys.Contains(x.Key);
            var documents = await _service.Call<List<Document>>(token, ApiRoutes.Documents.Filter, CallMethodType.Post, expected);

            if (PortalInfo.LogoFile != null && PortalInfo.LogoFile.Length > 0)
            {
                var doc = documents.Data.FirstOrDefault(x => x.Key == "Logo");

                if (doc != null)
                {
                    var memoryStream = new MemoryStream();
                    PortalInfo.LogoFile.CopyTo(memoryStream);

                    await _service.Call<Document>(token, ApiRoutes.Documents.UpdateJustContent, CallMethodType.Put, memoryStream.ToArray(), ("id", documents.Data.FirstOrDefault().Id));
                }
                else
                {
                    var file = new Document
                    {
                        Name = Path.GetFileName(PortalInfo.LogoFile.FileName),
                        ContentType = PortalInfo.LogoFile.ContentType,
                        Extension = Path.GetExtension(PortalInfo.LogoFile.FileName),
                        Size = PortalInfo.LogoFile.Length,
                        Key = "Logo",
                        ModuleId = ModuleType.PortalInfo,
                        ModuleRecordId = 1,
                        ModuleSubRecordId = 0,
                        UserId = userId,
                        Tags = "",
                        Order = 2
                    };

                    using (var target = new MemoryStream())
                    {
                        PortalInfo.LogoFile.CopyTo(target);
                        file.Content = target.ToArray();
                    }

                    var result = await _service.Call<Document>(token, ApiRoutes.Documents.Create, CallMethodType.Post, file);
                }
            }

            if (PortalInfo.FavIconFile != null && PortalInfo.FavIconFile.Length > 0)
            {
                var doc = documents.Data.FirstOrDefault(x => x.Key == "FavIcon");

                if (doc != null)
                {
                    var memoryStream = new MemoryStream();
                    PortalInfo.FavIconFile.CopyTo(memoryStream);

                    await _service.Call<Document>(token, ApiRoutes.Documents.UpdateJustContent, CallMethodType.Put, memoryStream.ToArray(), ("id", doc.Id));
                }
                else
                {
                    var file = new Document
                    {
                        Name = Path.GetFileName(PortalInfo.FavIconFile.FileName),
                        ContentType = PortalInfo.FavIconFile.ContentType,
                        Extension = Path.GetExtension(PortalInfo.FavIconFile.FileName),
                        Size = PortalInfo.FavIconFile.Length,
                        Key = "FavIcon",
                        ModuleId = ModuleType.PortalInfo,
                        ModuleRecordId = 1,
                        ModuleSubRecordId = 0,
                        UserId = userId,
                        Tags = "",
                        Order = 2
                    };

                    using (var target = new MemoryStream())
                    {
                        PortalInfo.FavIconFile.CopyTo(target);
                        file.Content = target.ToArray();
                    }

                    var result = await _service.Call<Document>(token, ApiRoutes.Documents.Create, CallMethodType.Post, file);
                }
            }

            if (PortalInfo.SmallLogoFile != null && PortalInfo.SmallLogoFile.Length > 0)
            {
                var doc = documents.Data.FirstOrDefault(x => x.Key == "SmallLogo");

                if (doc != null)
                {
                    var memoryStream = new MemoryStream();
                    PortalInfo.SmallLogoFile.CopyTo(memoryStream);

                    await _service.Call<Document>(token, ApiRoutes.Documents.UpdateJustContent, CallMethodType.Put, memoryStream.ToArray(), ("id", doc.Id));
                }
                else
                {
                    var file = new Document
                    {
                        Name = Path.GetFileName(PortalInfo.SmallLogoFile.FileName),
                        ContentType = PortalInfo.SmallLogoFile.ContentType,
                        Extension = Path.GetExtension(PortalInfo.SmallLogoFile.FileName),
                        Size = PortalInfo.SmallLogoFile.Length,
                        Key = "SmallLogo",
                        ModuleId = ModuleType.PortalInfo,
                        ModuleRecordId = 1,
                        ModuleSubRecordId = 0,
                        UserId = userId,
                        Tags = "",
                        Order = 2
                    };

                    using (var target = new MemoryStream())
                    {
                        PortalInfo.SmallLogoFile.CopyTo(target);
                        file.Content = target.ToArray();
                    }

                    var result = await _service.Call<Document>(token, ApiRoutes.Documents.Create, CallMethodType.Post, file);
                }
            }

            if (PortalInfo.DeveloperLogoFile != null && PortalInfo.DeveloperLogoFile.Length > 0)
            {
                var doc = documents.Data.FirstOrDefault(x => x.Key == "DeveloperLogo");

                if (doc != null)
                {
                    var memoryStream = new MemoryStream();
                    PortalInfo.DeveloperLogoFile.CopyTo(memoryStream);

                    await _service.Call<Document>(token, ApiRoutes.Documents.UpdateJustContent, CallMethodType.Put, memoryStream.ToArray(), ("id", doc.Id));
                }
                else
                {
                    var file = new Document
                    {
                        Name = Path.GetFileName(PortalInfo.DeveloperLogoFile.FileName),
                        ContentType = PortalInfo.DeveloperLogoFile.ContentType,
                        Extension = Path.GetExtension(PortalInfo.DeveloperLogoFile.FileName),
                        Size = PortalInfo.DeveloperLogoFile.Length,
                        Key = "DeveloperLogo",
                        ModuleId = ModuleType.PortalInfo,
                        ModuleRecordId = 1,
                        ModuleSubRecordId = 0,
                        UserId = userId,
                        Tags = "",
                        Order = 2
                    };

                    using (var target = new MemoryStream())
                    {
                        PortalInfo.DeveloperLogoFile.CopyTo(target);
                        file.Content = target.ToArray();
                    }

                    var result = await _service.Call<Document>(token, ApiRoutes.Documents.Create, CallMethodType.Post, file);
                }
            }

            var success = await _service.Call<Domain.Entities.Possibility.PortalInfo>(token, ApiRoutes.PortalInfos.Update, CallMethodType.Put, PortalInfo, ("Id", PortalInfo.Id));

            if (success.Succeeded)
            {
                Message = $"Update 'PortalInfo' successfully.";
                return RedirectToPage("Index");
            }
            else
            {
                ErrorMessage = $"'PortalInfo' cannot be updated, try another one.";
                return Page();
            }
        }
    }
}
