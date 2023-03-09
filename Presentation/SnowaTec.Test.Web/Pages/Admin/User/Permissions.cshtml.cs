using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Security;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Helper;
using SnowaTec.Test.Web.Contract;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;

namespace SnowaTec.Test.Web.Pages.Admin.User
{
    [Authorize]
    public class PermissionsModel : PageModel
    {
        [Inject]
        private ICallApiService _service { get; }

        [Inject]
        private IMapper _mapper { get; }

        [BindProperty]
        public long UserId { get; set; }

        [BindProperty]
        public long RoleId { get; set; }

        [BindProperty]
        public List<PermissionDto> Permissions { get; set; }

        public PermissionsModel(ICallApiService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task OnGet(long userId, long roleId)
        {
            UserId = userId;
            RoleId = roleId;

            Permissions = new List<PermissionDto>();

            var basePath = $"{AppDomain.CurrentDomain.FriendlyName}.Pages";

            var subPermissions = typeof(Program).Assembly.ExportedTypes.Where(x =>
                typeof(ISubPermission).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<ISubPermission>().ToList();

            var token = Request.Cookies["AccessToken"].ToString();

            Expression<Func<Domain.Entities.Security.Availability, bool>> expressionAvailability = x => x.RoleId == roleId;
            var availabilitie = await _service.Call<List<Domain.Entities.Security.Availability>>(token, ApiRoutes.Availabilities.Filter, CallMethodType.Post, expressionAvailability);

            if (availabilitie.Data.Count > 0)
            {
                var ids = availabilitie.Data.Select(x => x.SystemPartId).ToList();

                Expression<Func<SystemPart, bool>> expressionSystemPart = x => ids.Contains(x.Id);
                var systemParts = await _service.Call<List<SystemPart>>(token, ApiRoutes.SystemParts.Filter, CallMethodType.Post, expressionSystemPart);

                if (systemParts.Data.Any(x => x.AdditionalPermissions))
                {
                    var data = systemParts.Data.Where(x => x.AdditionalPermissions).ToList();
                    foreach (var x in data)
                    {
                        var path = x.EnglishTitle;
                        var parentId = x.ParentId;

                        while (parentId != null)
                        {
                            var find = systemParts.Data.FirstOrDefault(y => y.Id == parentId);
                            path = $"{find.EnglishTitle}.{path}";
                            parentId = find.ParentId;
                        }

                        path = $"{basePath}.{path}.SubPermission";

                        var permission = subPermissions.FirstOrDefault(x => x.ToString() == path);

                        if (permission != null)
                        {
                            var answer = await permission.GetPermissions(_service, _mapper, token, userId, roleId);

                            foreach (var item in answer)
                            {
                                var model = new List<PermissionItemDto>();

                                if (item.PermissionItems.Count > 0)
                                {
                                    model = ParentChildModelConvert.RecursiceContent(item.PermissionItems, null, "Key", "ParentKey", "SubPermissionItems");
                                }

                                item.PermissionItems = model;
                            }

                            Permissions.AddRange(answer);
                        }
                    }
                }
            }
        }

        public async Task<ActionResult> OnPostSaveAsync(long userId, long roleId, List<PermissionViewDto> data)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var result = new List<Response<PermissionItem>>();

                foreach (var row in data)
                {
                    Expression<Func<Permission, bool>> expressionPermission = x => x.UserId == userId && x.RoleId == roleId && x.Key == row.Key;
                    var permissions = await _service.Call<List<Permission>>(token, ApiRoutes.Permissions.Filter, CallMethodType.Post, expressionPermission);

                    var permission = new Permission();

                    if (permissions.Data.Count > 0)
                    {
                        permission = permissions.Data.FirstOrDefault();
                    }
                    else
                    {
                        var model = new Permission
                        {
                            UserId = userId,
                            RoleId = roleId,
                            Key = row.Key
                        };

                        var answer = await _service.Call<Permission>(token, ApiRoutes.Permissions.Create, CallMethodType.Post, model);

                        permission = answer.Data;
                    }

                    Expression<Func<PermissionItem, bool>> expressionPermissionItem = x => x.PermissionId == permission.Id;
                    var permissionItems = await _service.Call<List<PermissionItem>>(token, ApiRoutes.PermissionItems.Filter, CallMethodType.Post, expressionPermissionItem);

                    var existLicenses = new List<string>();

                    if (permissionItems.Data.Count > 0)
                    {
                        foreach (var item in permissionItems.Data)
                        {
                            if (!row.UndeterminedLicenses.Contains(item.Key.ToString()) && !row.SelectedLicenses.Contains(item.Key.ToString()))
                            {
                                result.Add(await _service.Call<PermissionItem>(token, ApiRoutes.PermissionItems.Delete, CallMethodType.Delete, model: null, ("id", item.Id), ("type", 4)));
                                continue;
                            }
                            else
                            {
                                existLicenses.Add(item.Key.ToString());
                            }

                            if (row.UndeterminedLicenses.Contains(item.Key.ToString()) && item.Undetermined == false)
                            {
                                item.Undetermined = true;

                                result.Add(await _service.Call<PermissionItem>(token, ApiRoutes.PermissionItems.Update, CallMethodType.Put, item, ("id", item.Id)));
                            }

                            if (row.SelectedLicenses.Contains(item.Key.ToString()) && item.Undetermined == true)
                            {
                                item.Undetermined = false;

                                result.Add(await _service.Call<PermissionItem>(token, ApiRoutes.PermissionItems.Update, CallMethodType.Put, item, ("id", item.Id)));
                            }
                        }
                    }

                    if (row.SelectedLicenses != null && row.SelectedLicenses.Count > 0)
                    {
                        row.SelectedLicenses.Where(x => !existLicenses.Contains(x)).ToList().ForEach(async x =>
                        {
                            var model = new PermissionItem
                            {
                                PermissionId = permission.Id,
                                Key = int.Parse(x),
                                Undetermined = false
                            };

                            result.Add(await _service.Call<PermissionItem>(token, ApiRoutes.PermissionItems.Create, CallMethodType.Post, model));
                        });
                    }

                    if (row.UndeterminedLicenses != null && row.UndeterminedLicenses.Count > 0)
                    {
                        row.UndeterminedLicenses.Where(x => !existLicenses.Contains(x)).ToList().ForEach(async x =>
                        {
                            var model = new PermissionItem
                            {
                                PermissionId = permission.Id,
                                Key = int.Parse(x),
                                Undetermined = true
                            };

                            result.Add(await _service.Call<PermissionItem>(token, ApiRoutes.PermissionItems.Create, CallMethodType.Post, model));
                        });
                    }
                }

                var succeededes = result.Select(x => x.Succeeded).Distinct().ToList();
                var messages = result.Select(x => x.Message).Distinct().ToList();
                var errors = new List<string>();
                result.Select(x => x.Errors).Distinct().ToList().ForEach(x => { errors.AddRange(x); });

                return new JsonResult(new Response<PermissionItem>
                {
                    Succeeded = succeededes.MostCommon(),
                    Message = String.Join(Environment.NewLine, messages),
                    Errors = errors
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<PermissionItem> { Message = "", Errors = new List<string> { ex.Message } });
            }
        }
    }
}
