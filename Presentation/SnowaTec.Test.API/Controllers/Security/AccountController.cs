using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Security;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Service.Contract.Security;

namespace SnowaTec.Test.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost(ApiRoutes.Accounts.Register)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            return Ok(await _userService.CreateAsync(request));
        }

        [HttpPut(ApiRoutes.Accounts.UpdateUserInfo)]
        public async Task<IActionResult> UpdateUserInfoAsync(long userId, ApplicationUser model)
        {
            return Ok(await _userService.UpdateUserInfoAsync(userId, model));
        }

        [HttpDelete(ApiRoutes.Accounts.Delete)]
        public async Task<IActionResult> DeleteAsync(long userId, int type)
        {
            return Ok(await _userService.DeleteAsync(userId, type));
        }

        [HttpPost(ApiRoutes.Accounts.Undelete)]
        public async Task<IActionResult> UndeleteAsync(long userId, int type)
        {
            return Ok(await _userService.UndeleteAsync(userId, type));
        }

        [HttpGet(ApiRoutes.Accounts.GetByUserId)]
        public async Task<IActionResult> GetByUserIdAsync(long userId)
        {
            return Ok(await _userService.GetByUserIdAsync(userId));
        }

        [HttpGet(ApiRoutes.Accounts.GetByUserName)]
        public async Task<IActionResult> GetByUserNameAsync(string username)
        {
            return Ok(await _userService.GetByUserNameAsync(username));
        }

        [HttpGet(ApiRoutes.Accounts.GetByPhoneNumber)]
        public async Task<IActionResult> GetByPhoneNumberAsync(string phoneNumber)
        {
            return Ok(await _userService.GetByPhoneNumberAsync(phoneNumber));
        }

        [HttpGet(ApiRoutes.Accounts.GetByAccessToken)]
        public async Task<IActionResult> GetByAccessTokenAsync(string accessToken)
        {
            return Ok(await _userService.GetByAccessTokenAsync(accessToken));
        }

        [HttpGet(ApiRoutes.Accounts.CheckExistingUsername)]
        [AllowAnonymous]
        public async Task<IActionResult> CheckExistingUsernameAsync(string username)
        {
            return Ok(await _userService.CheckExistingUsernameAsync(username));
        }

        [HttpPost(ApiRoutes.Accounts.LoginMobile)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginMobileAsync(string phone, string mobileCode, bool createUser = true)
        {
            return Ok(await _userService.LoginMobileAsync(phone, mobileCode, createUser));
        }

        [HttpPost(ApiRoutes.Accounts.VerifyMobile)]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyMobileAsync(VerifyRequest request)
        {
            return Ok(await _userService.VerifyMobileAsync(request));
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        [HttpPost(ApiRoutes.Accounts.Login)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(AuthenticationUserRequest request)
        {
            return Ok(await _userService.LoginUserAsync(request, GenerateIPAddress()));
        }

        [HttpPost(ApiRoutes.Accounts.RefreshToken)]
        public async Task<IActionResult> RefreshTokenAsync(string token)
        {
            return Ok(await _userService.RefreshTokenAsync(token, GenerateIPAddress()));
        }

        [HttpPost(ApiRoutes.Accounts.RevokeToken)]
        public async Task<IActionResult> RevokeToken(string token)
        {
            return Ok(_userService.RevokeToken(token, GenerateIPAddress()));
        }

        [HttpPost(ApiRoutes.Accounts.ForgotPassword)]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest model)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _userService.ForgotPasswordAsync(model, origin));
        }

        [HttpPost(ApiRoutes.Accounts.ResetPassword)]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest model)
        {
            return Ok(await _userService.ResetPasswordAsync(model));
        }


        [HttpGet(ApiRoutes.Accounts.GetUserRoles)]
        public async Task<IActionResult> GetUserRolesAsync(long userId)
        {
            return Ok(await _userService.GetUserRolesAsync(userId));
        }

        [HttpPost(ApiRoutes.Accounts.AddUserNameToRoleName)]
        public async Task<IActionResult> AddUserNameToRoleName(string userName, string roleName)
        {
            return Ok(await _userService.AddUserNameToRoleNameAsync(userName, roleName));
        }

        [HttpPost(ApiRoutes.Accounts.RemoveUserNameFromRoleName)]
        public async Task<IActionResult> RemoveUserNameToRoleName(string userName, string roleName)
        {
            return Ok(await _userService.RemoveUserNameFromRoleNameAsync(userName, roleName));
        }

        [HttpPost(ApiRoutes.Accounts.AddUserIdToRoleId)]
        public async Task<IActionResult> AddUserIdToRoleId(long userId, long roleId)
        {
            return Ok(await _userService.AddUserIdToRoleIdAsync(userId, roleId));
        }

        [HttpPost(ApiRoutes.Accounts.RemoveUserIdFromRoleId)]
        public async Task<IActionResult> RemoveUserIdToRoleId(long userId, long roleId)
        {
            return Ok(await _userService.RemoveUserIdFromRoleIdAsync(userId, roleId));
        }
    }
}