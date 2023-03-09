using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Security;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Extensions;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;

namespace SnowaTec.Test.Web.Pages.Admin.User
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

        public async Task<JsonResult> OnPostAsync(DTParameters dtModel)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var result = await _service.Call<DTResult<ApplicationUser>>(token, ApiRoutes.Users.Pagination, Enums.CallMethodType.Post, dtModel);

            if (!User.IsInRole(Roles.SuperAdmin.ToString()))
            {
                result.Data.data.Remove(result.Data.data.FirstOrDefault(x => x.NormalizedUserName == Roles.SuperAdmin.ToString().ToUpper()));
                result.Data.recordsTotal -= 1;
                if(result.Data.recordsFiltered > 0)
                    result.Data.recordsFiltered -= 1;
            }

            return new JsonResult(result.Data);
        }

        public async Task<PartialViewResult> OnGetCreateUpdateAsync(int? id)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var model = new UserDto();

            if (id > 0)
            {
                var result = await _service.Call<ApplicationUser>(token, ApiRoutes.Users.Get, Enums.CallMethodType.Get, model: null, ("id", id));

                if (result.Succeeded)
                {
                    var userRoles = await _service.Call<List<RoleDto>>(token, ApiRoutes.Accounts.GetUserRoles, Enums.CallMethodType.Get, model: null, ("userId", id));

                    model.Id = result.Data.Id;
                    model.Prefix = result.Data.Prefix;
                    model.FullName = result.Data.FullName;
                    model.NickName = result.Data.NickName;
                    model.PhoneNumber = result.Data.PhoneNumber;
                    model.Active = !result.Data.LockoutEnabled;
                    model.UserName = result.Data.UserName;
                    model.Password = "";
                    model.ConfirmPassword = "";
                    model.Roles = userRoles.Data;
                }
            }

            return LoadPartialView.Show<UserDto>("_UserEdit", model, ViewData, TempData);
        }

        public async Task<ActionResult> OnPostSaveAsync(UserDto request)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                if (request.Id != 0)
                {
                    ModelState.Remove("Username");
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");
                    ModelState.Remove("Roles");
                }

                if (ModelState.IsValid)
                {
                    //var result = new Response<bool>();

                    if (request.Id != 0)
                    {
                        var user = await _service.Call<ApplicationUser>(token, ApiRoutes.Users.Get, Enums.CallMethodType.Get, model: null, ("id", request.Id));

                        user.Data.Prefix = request.Prefix;
                        user.Data.FullName = request.FullName;
                        user.Data.NickName = request.NickName;
                        user.Data.PhoneNumber = request.PhoneNumber;
                        user.Data.LockoutEnabled = !request.Active;

                        var result = await _service.Call<ApplicationUser>(token, ApiRoutes.Users.Update, Enums.CallMethodType.Put, user.Data, ("id", request.Id));

                        var userRoles = await _service.Call<List<RoleDto>>(token, ApiRoutes.Accounts.GetUserRoles, Enums.CallMethodType.Get, model: null, ("userId", user.Data.Id));
                        if (userRoles.Succeeded && userRoles.Data.Count > 0)
                        {
                            userRoles.Data.ForEach(async x =>
                            {
                                if (!request.Roles.Any(z => z.RoleId == x.RoleId))
                                {
                                    await _service.Call<bool>(token, ApiRoutes.Accounts.RemoveUserIdFromRoleId, Enums.CallMethodType.Post, model: null, ("userId", user.Data.Id), ("roleId", x.RoleId));
                                }
                                else
                                {
                                    var find = request.Roles.FirstOrDefault(z => z.RoleId == x.RoleId);
                                    request.Roles.Remove(find);
                                }
                            });
                        }

                        request.Roles.ForEach(async x =>
                        {
                            await _service.Call<bool>(token, ApiRoutes.Accounts.AddUserIdToRoleId, Enums.CallMethodType.Post, model: null, ("userId", user.Data.Id), ("roleId", x.RoleId));
                        });

                        return new JsonResult(result);
                    }
                    else
                    {
                        var existUserName = await _service.Call<bool>("", ApiRoutes.Accounts.CheckExistingUsername, Enums.CallMethodType.Get, model: null, ("username", request.UserName));
                        if (existUserName.Data)
                        {
                            return new JsonResult(existUserName);
                        }

                        var roleIds = request.Roles.Select(x => x.RoleId).ToList();
                        Expression<Func<ApplicationRole, bool>> expression = x => roleIds.Contains(x.Id);
                        var roles = await _service.Call<List<ApplicationRole>>(token, ApiRoutes.Roles.Filter, CallMethodType.Post, expression);

                        var roleNames = new List<string>();
                        roleNames = roles.Data.Select(x => x.Name).ToList();

                        var registerUser = new RegisterRequest
                        {
                            Username = request.UserName,
                            PhoneNumber = request.PhoneNumber,
                            //Email = "",
                            Roles = roleNames,
                            Password = request.Password,
                            ConfirmPassword = request.ConfirmPassword,
                            Prefix = request.Prefix,
                            FullName = request.FullName,
                            NickName = request.NickName,
                            LockoutEnabled = !request.Active
                        };

                        var result = await _service.Call<ApplicationUser>("", ApiRoutes.Accounts.Register, CallMethodType.Post, registerUser);

                        return new JsonResult(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message, error = JsonConvert.SerializeObject(ex) });
            }

            return null;
        }

        public async Task<ActionResult> OnPostDeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<ApplicationUser>(token, ApiRoutes.Users.Delete, Enums.CallMethodType.Delete, model: null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<ApplicationUser> { Message = "", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<ActionResult> OnPostUndeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<ApplicationUser>(token, ApiRoutes.Users.UnDelete, Enums.CallMethodType.Post, model: null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<ApplicationUser> { Message = "", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<PartialViewResult> OnGetRoleRowAsync(long id, int? rowNo)
        {
            var model = new RoleDto
            {
                Id = id,
                Order = rowNo ?? 0
            };

            var token = Request.Cookies["AccessToken"].ToString();

            if (id > 0)
            {
                var result = await _service.Call<ApplicationRole>(token, ApiRoutes.Roles.Get, CallMethodType.Get, null, ("id", id));
                model = _mapper.Map<RoleDto>(result.Data);
            }

            return LoadPartialView.Show<RoleDto>("_UserRoleRow", model, ViewData, TempData);
        }

        public async Task<JsonResult> OnGetAdditionalPermissions(long roleId)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var result = false;

            Expression<Func<Domain.Entities.Security.Availability, bool>> expressionAvailability = x => x.RoleId == roleId;
            var availabilitie = await _service.Call<List<Domain.Entities.Security.Availability>>(token, ApiRoutes.Availabilities.Filter, CallMethodType.Post, expressionAvailability);

            if (availabilitie.Data.Count > 0)
            {
                var ids = availabilitie.Data.Select(x => x.SystemPartId).ToList();

                Expression<Func<SystemPart, bool>> expressionSystemPart = x => ids.Contains(x.Id) && x.AdditionalPermissions;
                var systemParts = await _service.Call<List<SystemPart>>(token, ApiRoutes.SystemParts.Filter, CallMethodType.Post, expressionSystemPart);

                if (systemParts.Data.Count > 0)
                    result = true;
            }

            return new JsonResult(result);
        }
    }
}
