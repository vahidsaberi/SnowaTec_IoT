using IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Security;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Web.Interfaces.Base;

namespace SnowaTec.Test.Web.Pages.Authentication
{
    public class ProfileModel : PageModel
    {
        [Inject]
        private ICallApiService _service { get; }

        [BindProperty]
        public ApplicationUser InputUser { get; set; }

        [BindProperty]
        public ResetPasswordRequest InputPassword { get; set; }

        public ProfileModel(ICallApiService service)
        {
            _service = service;
        }

        #region Tools
        private async Task<ApplicationUser> GetUserProfile()
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _service.Call<ApplicationUser>(token, ApiRoutes.Accounts.GetByUserId, Enums.CallMethodType.Get, model: null, ("userId", userId));

            if (user.Data != null)
                return user.Data;

            return null;
        }
        #endregion

        public async Task<IActionResult> OnGet()
        {
            InputUser = await GetUserProfile();
            return Page();
        }

        public async Task<JsonResult> OnPostProfileAsync(ApplicationUser request)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var user = await _service.Call<ApplicationUser>(token, ApiRoutes.Accounts.GetByUserId, Enums.CallMethodType.Get, null, ("userId", request.Id));
                if (user.Data != null)
                {
                    user.Data.Prefix = request.Prefix;
                    user.Data.FullName = request.FullName;
                    user.Data.NickName = request.NickName;

                    var result = await _service.Call<bool>(token, ApiRoutes.Accounts.UpdateUserInfo, Enums.CallMethodType.Put, user.Data, ("userId", user.Data.Id));

                    return new JsonResult(new { result.Succeeded, result.Message, result.Errors });
                }

                return new JsonResult(new { user.Succeeded, user.Message, user.Errors });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Succeeded = false, Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<JsonResult> OnPostPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                if (InputPassword.NewPassword.ToLower().Trim().Equals(InputPassword.CurrentPassword.ToLower().Trim()))
                {
                    return new JsonResult(new { Succeeded = false, Errors = new List<string> { "گذرواژه جدید همان گذرواژه قدیمی شماست!" } });
                }

                if (!InputPassword.NewPassword.ToLower().Trim().Equals(InputPassword.ConfirmNewPassword.ToLower().Trim()))
                {
                    return new JsonResult(new { Succeeded = false, Errors = new List<string> { "گذرواژه جدید با تکرار گذرواژه یکسان نیست!" } });
                }

                request.UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var username = User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.PreferredUserName).Value;

                var model = new ForgotPasswordRequest { Username = username };

                var getToken = await _service.Call<string>("", ApiRoutes.Accounts.ForgotPassword, Enums.CallMethodType.Post, model);

                if (!getToken.Succeeded)
                {
                    return new JsonResult(new { Succeeded = false, Errors = new List<string> { "در دریافت کد تغییر گذرواژه خطایی رخ داد!" } });
                }

                request.Token = getToken.Data;

                var result = await _service.Call<string>("", ApiRoutes.Accounts.ResetPassword, Enums.CallMethodType.Post, request);

                return new JsonResult(new { result.Succeeded, result.Message, result.Errors });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Succeeded = false, Errors = new List<string> { ex.Message } });
            }
        }
    }
}
