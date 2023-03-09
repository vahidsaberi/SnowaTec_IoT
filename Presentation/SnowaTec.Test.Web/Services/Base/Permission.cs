using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;
using System.Security.Claims;

namespace SnowaTec.Test.Web.Services.Base
{
    public class Permission : IPermission
    {
        private readonly ICallApiService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private long UserId { get; set; }
        private List<string> RoleList { get; set; }
        private List<ApplicationRole> Roles { get; set; }

        private List<SystemPart> AccessSystemParts { get; set; }

        private bool _retentionOfPreviousData { get; set; }

        public Permission(ICallApiService service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;

            if (_httpContextAccessor.HttpContext.Request.Cookies["AccessToken"] == null)
                return;

            var token = httpContextAccessor.HttpContext.Request.Cookies["AccessToken"].ToString();

            UserId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var portalInfo = _service.Call<PortalInfo>(token, ApiRoutes.PortalInfos.Get, CallMethodType.Get, null, ("Id", 1)).Result;

            if (portalInfo.Data == null)
                this._retentionOfPreviousData = false;
            else
                this._retentionOfPreviousData = portalInfo.Data.RetentionOfPreviousData;

            RoleList = httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList();

            AccessSystemParts = new List<SystemPart>();
            Roles = new List<ApplicationRole>();

            var items = new List<long>();

            if (RoleList.Contains(SnowaTec.Test.Domain.Enum.Roles.SuperAdmin.ToString()))
            {
                AccessSystemParts = _service.Call<List<SystemPart>>(token, ApiRoutes.SystemParts.GetAll, CallMethodType.Get).Result.Data;
                return;
            }

            RoleList.ForEach(x =>
            {
                var role = _service.Call<ApplicationRole>(token, ApiRoutes.Roles.GetByName, CallMethodType.Get, model: null, ("name", x)).Result.Data;

                Expression<Func<Availability, bool>> expected = x => x.RoleId == role.Id;
                var availabilities = _service.Call<List<Availability>>(token, ApiRoutes.Availabilities.Filter, CallMethodType.Post, expected).Result;

                if (availabilities.Succeeded && availabilities.Data.Count > 0)
                {
                    items.AddRange(availabilities.Data.Select(x => x.SystemPartId).ToList());
                }

                Roles.Add(role);
            });

            Expression<Func<SystemPart, bool>> expected = x => items.Contains(x.Id);
            var systemParts = _service.Call<List<SystemPart>>(token, ApiRoutes.SystemParts.Filter, CallMethodType.Post, expected).Result;

            AccessSystemParts = systemParts.Data.ToList();
        }

        public async Task<bool> HasAccess(string systemPart, PermissionType permission)
        {
            var find = AccessSystemParts.FirstOrDefault(x => x.EnglishTitle.ToLower() == systemPart.ToLower());
            if (find == null) return false;

            return AccessSystemParts.Any(x => x.ParentId == find.Id && x.MenuType == MenuType.License && x.PersianTitle == permission.ToString());
        }

        public async Task<List<SystemPart>> GetAllAccesses()
        {
            return await Task.Run(() => AccessSystemParts);
        }

        public async Task<List<long>> GetSubPermissions(string key)
        {
            var result = new List<long>();

            var token = _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"].ToString();

            Expression<Func<Domain.Entities.Security.Permission, bool>> expressionPermission = x => x.UserId == this.UserId && x.Key.ToLower() == key.ToLower();
            var permissions = await _service.Call<List<Domain.Entities.Security.Permission>>(token, ApiRoutes.Permissions.Filter, CallMethodType.Post, expressionPermission);

            if (permissions.Data.Count > 0)
            {
                var permissionId = permissions.Data.FirstOrDefault().Id;

                Expression<Func<PermissionItem, bool>> expressionPermissionItem = x => x.PermissionId == permissionId;
                var permissionItems = await _service.Call<List<PermissionItem>>(token, ApiRoutes.PermissionItems.Filter, CallMethodType.Post, expressionPermissionItem);

                if (permissionItems.Data.Count > 0)
                {
                    result = permissionItems.Data.Select(x => x.Key).ToList();
                }
            }

            return result;
        }

        public async Task<bool> RetentionOfPreviousData()
        {
            return this._retentionOfPreviousData;
        }
    }
}
