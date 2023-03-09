using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Identity;
using SnowaTec.Test.Service.Contract.Security;

namespace SnowaTec.Test.Service.Implementation.Security
{
    public class RoleService : IRoleService
    {
        private readonly IdentityContext _identityContext;

        public RoleService(IdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<Response<ApplicationRole>> GetAsync(long id)
        {
            var data = await _identityContext.Roles.FindAsync(id);
            var model = new Response<ApplicationRole>(data, "نقش کاربری با موفقیت بارگذاری شد.");
            return model;
        }

        public async Task<Response<List<ApplicationRole>>> GetAllAsync()
        {
            var data = await _identityContext.Roles.ToListAsync();
            var model = new Response<List<ApplicationRole>>(data, "نقش های کاربری با موفقیت بارگذاری شد.");
            return model;
        }

        public async Task<Response<List<ApplicationUser>>> GetUsersInRoleAsync(string roleName)
        {
            var role = await _identityContext.Roles.FindAsync(roleName);

            var userRoles = await _identityContext.UserRoles.Where(x => x.RoleId == role.Id).ToListAsync();

            var userIds = userRoles.Select(x => x.UserId).ToList();

            var users = await _identityContext.Users.Where(x => userIds.Contains(x.Id)).ToListAsync();

            return new Response<List<ApplicationUser>>(users, $"بارگذاری کاربران درون نقش کاربری {roleName} با موفقیت انجام شد.");
        }

        public async Task<Response<Dictionary<string, List<ApplicationUser>>>> GetAllUsersAndRoleAsync()
        {
            var usersInRole = new Dictionary<string, List<ApplicationUser>>();

            var users = await _identityContext.Users.ToListAsync();

            var roles = await _identityContext.Roles.ToListAsync();

            var userRoles = await _identityContext.UserRoles.ToListAsync();

            roles.ForEach(x =>
            {
                var userIds = userRoles.Where(y => y.RoleId == x.Id).Select(y => y.UserId).ToList();
                var userList = users.Where(y => userIds.Contains(y.Id)).ToList();

                usersInRole.Add(x.Name, userList);
            });

            return new Response<Dictionary<string, List<ApplicationUser>>>(usersInRole, "بارگذاری کاربران و نقش های کاربری با موفقیت انجام شد.");
        }

        public async Task<Response<ApplicationRole>> CreateAsync(ApplicationRole model)
        {
            if (_identityContext.Roles.Any(x => x.Name.ToLower() == model.Name.ToLower()))
            {
                return new Response<ApplicationRole> { Data = model, Succeeded = false, Message = "نقش کاربری وارد شده قبلا ثبت شده است" };
            }

            _identityContext.Roles.Add(model);
            var result = await _identityContext.SaveChangesAsync();

            if (result > 0)
                return new Response<ApplicationRole>(model, "ایجاد نقش کاربری با موفقیت انجام شد.");

            return new Response<ApplicationRole> { Data = model, Succeeded = false, Message = "خطا در ایجاد نقش کاربری" };
        }

        public async Task<Response<ApplicationRole>> UpdateAsync(ApplicationRole model)
        {
            _identityContext.Roles.Update(model);
            var result = await _identityContext.SaveChangesAsync();

            if (result > 0)
                return new Response<ApplicationRole>(model, "بروزرسانی نقش کاربری با موفقیت انجام شد.");

            return new Response<ApplicationRole>(model, "خطا در بروزرسانی نقش کاربری");
        }

        public async Task<Response<ApplicationRole>> GetByNameAsync(string roleName)
        {
            var role = await _identityContext.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == roleName.ToLower());

            return new Response<ApplicationRole>(role, "بارگذاری نقش کاربری با موفقیت انجام شد.");
        }

        public async Task<Response<List<ApplicationRole>>> Filter(string query)
        {
            try
            {
                var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(query) as Remote.Linq.Expressions.LambdaExpression;

                var localexpression = LinqExpressionHelper.ToLinqExpression<ApplicationRole, bool>(expression);

                var predicate = localexpression.Compile();

                var modelList = _identityContext.Roles.Where(predicate).ToList();

                if (modelList == null) return new Response<List<ApplicationRole>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                return new Response<List<ApplicationRole>>(modelList, "بارگذاری اطلاعات با موفقیت انجام شد.");
            }
            catch (System.Exception ex)
            {
                return new Response<List<ApplicationRole>> { Succeeded = false, Errors = new List<string> { ex.Message } };
            }
        }
    }
}
