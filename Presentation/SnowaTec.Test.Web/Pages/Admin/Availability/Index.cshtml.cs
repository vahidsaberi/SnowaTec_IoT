using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Security;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Web.Contract;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Extensions;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;

namespace SnowaTec.Test.Web.Pages.Admin.Availability
{
    public class RoleDataTableModel
    {
        public long Id { get; set; }
        public bool Deleted { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public int UserCount { get; set; }
    }

    [Authorize]
    public class IndexModel : PageModel
    {
        #region Properties
        [Inject]
        public ICallApiService _service { get; }

        [Inject]
        private IMapper _mapper { get; }
        #endregion

        #region Constructor
        public IndexModel(ICallApiService service, IMapper mapper)
        {
            this._service = service;
            this._mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> OnPostAsync(DTParameters dtModel)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var roles = await _service.Call<List<ApplicationRole>>(token, ApiRoutes.Roles.GetAll, CallMethodType.Get);
            var users = await _service.Call<Dictionary<string, List<ApplicationUser>>>(token, ApiRoutes.Roles.GetAllUsersAndRole, CallMethodType.Get);

            var data = new List<RoleDataTableModel>();

            var id = 1;

            foreach (var x in roles.Data)
            {
                if (x.Name == Roles.SuperAdmin.ToString())
                {
                    continue;
                }

                data.Add(new RoleDataTableModel
                {
                    Id = id++,
                    Deleted = false,
                    RoleId = x.Id,
                    RoleName = x.Name,
                    UserCount = users.Data[x.Name].Count
                });
            }

            return new JsonResult(new { draw = dtModel.Draw, recordsTotal = data.Count, recordsFiltered = 0, data, error = "" });
        }

        public async Task<PartialViewResult> OnGetCreateUpdateAsync(long id, long? parentId)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var model = new AvailabilityRoleDto
            {
                Id = id
            };

            if (id > 0)
            {
                var result = await _service.Call<ApplicationRole>(token, ApiRoutes.Roles.Get, CallMethodType.Get, model: null, ("id", id));
                if (result.Data != null)
                {
                    model.Id = result.Data.Id;
                    model.Name = result.Data.Name;
                }
            }

            return LoadPartialView.Show<AvailabilityRoleDto>("_AvailabilityEdit", model, ViewData, TempData);
        }

        public async Task<PartialViewResult> OnGetSystemPartsAsync()
        {
            var systemPart = await _service.Call<List<Domain.Entities.Security.SystemPart>>(Request.Cookies["AccessToken"].ToString(), ApiRoutes.SystemParts.GetAll, Enums.CallMethodType.Get);

            var model = new List<Domain.Entities.Security.SystemPart>();

            if (systemPart.Succeeded && systemPart.Data.Count > 0)
            {
                model = ParentChildModelConvert.RecursiceContent(systemPart.Data, null, "Id", "ParentId", "SubSystemParts");
            }

            model.Remove(model.FirstOrDefault(x => x.EnglishTitle == Roles.SuperAdmin.ToString()));

            return LoadPartialView.Show<List<Domain.Entities.Security.SystemPart>>("_Tree", model, ViewData, TempData);
        }

        public async Task<ActionResult> OnPostSaveAsync(AvailabilityRoleDto request)
        {
            try
            {
                request.Name = request.Name.Replace(" ", "_");

                var token = Request.Cookies["AccessToken"].ToString();

                var selectedLicenses = new List<string>();
                var undeterminedLicenses = new List<string>();

                if (!string.IsNullOrEmpty(request.SelectedLicenses))
                    selectedLicenses = request.SelectedLicenses.Split(',').ToList();

                if (!string.IsNullOrEmpty(request.UndeterminedLicenses))
                    undeterminedLicenses = request.UndeterminedLicenses.Split(',').ToList();

                var answer = new Response<ApplicationRole>(null, "عملیات با موفقیت انجام شد.");

                if (request.Id > 0)
                {
                    var role = await _service.Call<ApplicationRole>(token, ApiRoutes.Roles.Get, CallMethodType.Get, model: null, ("Id", request.Id));
                    if (role.Data.Name != request.Name)
                    {
                        role.Data.Name = request.Name;
                        role.Data.NormalizedName = request.Name;

                        answer = await _service.Call<ApplicationRole>(token, ApiRoutes.Roles.Update, CallMethodType.Put, role.Data, ("Id", request.Id));
                    }

                    Expression<Func<Domain.Entities.Security.Availability, bool>> expected = x => x.RoleId == request.Id;
                    var availabilities = await _service.Call<List<Domain.Entities.Security.Availability>>(token, ApiRoutes.Availabilities.Filter, CallMethodType.Post, expected);

                    var existLicenses = new List<string>();

                    if (availabilities.Succeeded && availabilities.Data.Count > 0)
                    {
                        availabilities.Data.ForEach(async x =>
                        {
                            if (!undeterminedLicenses.Contains(x.SystemPartId.ToString()) && !selectedLicenses.Contains(x.SystemPartId.ToString()))
                            {
                                var result = await _service.Call<Domain.Entities.Security.Availability>(token, ApiRoutes.Availabilities.Delete, CallMethodType.Delete, model: null, ("id", x.Id), ("type", 4));
                            }
                            else
                            {
                                existLicenses.Add(x.SystemPartId.ToString());
                            }

                            if (undeterminedLicenses.Contains(x.SystemPartId.ToString()) && x.Undetermined == false)
                            {
                                x.Undetermined = true;

                                var result = await _service.Call<Domain.Entities.Security.Availability>(token, ApiRoutes.Availabilities.Update, CallMethodType.Put, x, ("id", x.Id));
                            }

                            if (selectedLicenses.Contains(x.SystemPartId.ToString()) && x.Undetermined == true)
                            {
                                x.Undetermined = false;

                                var result = await _service.Call<Domain.Entities.Security.Availability>(token, ApiRoutes.Availabilities.Update, CallMethodType.Put, x, ("id", x.Id));
                            }
                        });
                    }

                    selectedLicenses.Where(x => !existLicenses.Contains(x)).ToList().ForEach(async x =>
                    {
                        var model = new Domain.Entities.Security.Availability
                        {
                            RoleId = request.Id,
                            SystemPartId = int.Parse(x),
                            Undetermined = false
                        };

                        var result = await _service.Call<Domain.Entities.Security.Availability>(token, ApiRoutes.Availabilities.Create, CallMethodType.Post, model);
                    });

                    undeterminedLicenses.Where(x => !existLicenses.Contains(x)).ToList().ForEach(async x =>
                    {
                        var model = new Domain.Entities.Security.Availability
                        {
                            RoleId = request.Id,
                            SystemPartId = int.Parse(x),
                            Undetermined = true
                        };

                        var result = await _service.Call<Domain.Entities.Security.Availability>(token, ApiRoutes.Availabilities.Create, CallMethodType.Post, model);
                    });
                }
                else
                {
                    var role = new ApplicationRole
                    {
                        Id = request.Id,
                        Name = request.Name,
                        NormalizedName = request.Name
                    };

                    answer = await _service.Call<ApplicationRole>(token, ApiRoutes.Roles.Create, CallMethodType.Post, role);

                    if (answer.Succeeded && selectedLicenses.Count > 0)
                    {
                        selectedLicenses.ToList().ForEach(async x =>
                        {
                            var model = new Domain.Entities.Security.Availability
                            {
                                RoleId = answer.Data.Id,
                                SystemPartId = int.Parse(x),
                                Undetermined = false
                            };

                            var result = await _service.Call<Domain.Entities.Security.Availability>(token, ApiRoutes.Availabilities.Create, CallMethodType.Post, model);
                        });

                        undeterminedLicenses.ToList().ForEach(async x =>
                        {
                            var model = new Domain.Entities.Security.Availability
                            {
                                RoleId = request.Id,
                                SystemPartId = int.Parse(x),
                                Undetermined = true
                            };

                            var result = await _service.Call<Domain.Entities.Security.Availability>(token, ApiRoutes.Availabilities.Create, CallMethodType.Post, model);
                        });
                    }
                }

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<ApplicationRole> { Data = null, Succeeded = false, Message = "", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<JsonResult> OnGetLicensesAsync(long roleId)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            Expression<Func<Domain.Entities.Security.Availability, bool>> expected = x => x.RoleId == roleId && x.Undetermined == false;
            var availabilities = await _service.Call<List<Domain.Entities.Security.Availability>>(token, ApiRoutes.Availabilities.Filter, CallMethodType.Post, expected);

            var licenses = new List<long>();

            if (availabilities.Succeeded && availabilities.Data.Count > 0)
            {
                licenses = availabilities.Data.Select(x => x.SystemPartId).ToList();
            }

            return new JsonResult(licenses);
        }
        #endregion
    }
}
