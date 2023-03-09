using AutoMapper;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Security;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;

namespace SnowaTec.Test.Web.Pages.SmartAppliances
{
    public enum SubPermissionType
    {
        Test_Sections,
    }

    public class SubPermission : ISubPermission
    {
        public async Task<List<PermissionDto>> GetPermissions(ICallApiService _service, IMapper _mapper, string token, long userId, long roleId)
        {
            var permissionDtos = new List<PermissionDto>();

            var sectionPermission = new PermissionDto
            {
                UserId = userId,
                RoleId = roleId,
                Title = "دسترسی فضاها",
                Key = SubPermissionType.Test_Sections.ToString()
            };

            var sections = await _service.Call<List<Domain.Entities.Test.Section>>(token, ApiRoutes.Sections.GetAll, CallMethodType.Get);
            sections.Data.ForEach(x =>
            {
                sectionPermission.PermissionItems.Add(new PermissionItemDto
                {
                    Key = x.Id,
                    ParentKey = x.ParentId,
                    Title = x.Title
                });
            });

            permissionDtos.Add(sectionPermission);

            var permissionKeys = new List<string> { SubPermissionType.Test_Sections.ToString() };

            Expression<Func<Domain.Entities.Security.Permission, bool>> expressionPermission = x => x.UserId == userId && x.RoleId == roleId && permissionKeys.Contains(x.Key);
            var permissions = await _service.Call<List<Domain.Entities.Security.Permission>>(token, ApiRoutes.Permissions.Filter, CallMethodType.Post, expressionPermission);

            if (permissions.Data.Count > 0)
            {
                var ids = permissions.Data.Select(x => x.Id).ToList();

                Expression<Func<Domain.Entities.Security.PermissionItem, bool>> expressionPermissionItem = x => ids.Contains(x.PermissionId);
                var permissionItems = await _service.Call<List<Domain.Entities.Security.PermissionItem>>(token, ApiRoutes.PermissionItems.Filter, CallMethodType.Post, expressionPermissionItem);

                if (permissionItems.Data.Count > 0)
                {
                    foreach (var item in permissions.Data)
                    {
                        var selectedPermission = permissionDtos.FirstOrDefault(x => x.Key == item.Key);

                        var subs = selectedPermission.PermissionItems;

                        _mapper.Map(item, selectedPermission);

                        selectedPermission.PermissionItems = subs;

                        permissionItems.Data.Where(x => x.PermissionId == item.Id).ToList().ForEach(x =>
                        {
                            var find = selectedPermission.PermissionItems.FirstOrDefault(y => y.Key == x.Key);
                            _mapper.Map(x, find);
                            find.Selected = true;
                        });
                    }
                }
            }

            return permissionDtos;
        }
    }
}
