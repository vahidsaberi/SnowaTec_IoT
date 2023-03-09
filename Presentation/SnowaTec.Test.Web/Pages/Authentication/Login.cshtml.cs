using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Security;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Security.Claims;

namespace SnowaTec.Test.Web.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        [Inject]
        private ICallApiService _service { get; }

        [BindProperty]
        public AuthenticationUserRequest UserInput { get; set; }

        [BindProperty]
        public AuthenticationMobileRequest MobileInput { get; set; }

        [BindProperty]
        public PortalInfo PortalInfo { get; set; }

        [TempData]
        public string Message { get; set; }

        public LoginModel(ICallApiService service)
        {
            _service = service;

            PortalInfo = _service.Call<PortalInfo>("", ApiRoutes.PortalInfos.Get, Enums.CallMethodType.Get, model: null, ("Id", 1)).Result.Data;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var authResult = new Response<AuthenticationResponse>();

                if (!string.IsNullOrWhiteSpace(MobileInput.PhoneNumber) && !string.IsNullOrWhiteSpace(MobileInput.VerifyCode))
                {
                    var model = new VerifyRequest
                    {
                        PhoneNumber = MobileInput.PhoneNumber,
                        VerifyCode = MobileInput.VerifyCode
                    };

                    authResult = await _service.Call<AuthenticationResponse>("", ApiRoutes.Accounts.VerifyMobile, Enums.CallMethodType.Post, model);
                }
                else
                {
                    authResult = await _service.Call<AuthenticationResponse>("", ApiRoutes.Accounts.Login, Enums.CallMethodType.Post, UserInput);
                }

                if (authResult.Data != null && authResult.Data.IsVerified)
                {
                    #region Building Up Identity For Authenticated User
                    var UserClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, authResult.Data.Id.ToString()),
                                new Claim(JwtClaimTypes.PreferredUserName, authResult.Data.Username),
                                new Claim(ClaimTypes.Name, authResult.Data.FullName??""),
                                new Claim(ClaimTypes.GivenName, authResult.Data.NickName??"")
                            };

                    authResult.Data.Roles.ForEach(x =>
                    {
                        UserClaims.Add(new Claim(ClaimTypes.Role, x.RoleName));
                    });

                    var UserClaimsIdentity = new ClaimsIdentity(UserClaims, "Password");
                    #endregion

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                  new ClaimsPrincipal(UserClaimsIdentity),
                                                  new AuthenticationProperties
                                                  {
                                                      IsPersistent = UserInput.RememberMe,
                                                      RedirectUri = returnUrl
                                                  });

                    HttpContext.Response.Cookies.Append("AccessToken",
                                                        authResult.Data.JWToken,
                                                        new CookieOptions
                                                        {
                                                            HttpOnly = true,
                                                            SameSite = SameSiteMode.Strict
                                                        });
                    HttpContext.Response.Cookies.Append("RefreshToken",
                                                        authResult.Data.RefreshToken,
                                                        new CookieOptions
                                                        {
                                                            HttpOnly = true,
                                                            SameSite = SameSiteMode.Strict
                                                        });

                    if (returnUrl != null && returnUrl != "/")
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        var roleNames = authResult.Data.Roles.Select(x => x.RoleName).ToList();

                        if (roleNames.Contains(Domain.Enum.Roles.SuperAdmin.ToString()))
                            return RedirectToPage("/SuperAdmin/Dashboard");

                        if (roleNames.Contains(Domain.Enum.Roles.Admin.ToString()))
                            return RedirectToPage("/Admin/Dashboard");

                        if (roleNames.Contains(Domain.Enum.Roles.Moderator.ToString()))
                            return RedirectToPage("/Moderator/Dashboard");

                        if (roleNames.Contains(Domain.Enum.Roles.Basic.ToString()))
                            return RedirectToPage("/Basic/Dashboard");

                        return RedirectToPage("/Index");
                    }
                }
                else
                    Message = authResult.Errors != null ? String.Join(" | ", authResult.Errors.ToArray()) : "Failed sign in.";
            }
            return RedirectToPage();
        }

        public async Task<JsonResult> OnPostCreateVerifyCodeAsync(string phoneNumber)
        {
            try
            {
                var result = await _service.Call<bool>("", ApiRoutes.Accounts.LoginMobile, Enums.CallMethodType.Post, model: null, ("phone", phoneNumber), ("mobileCode", "0101"), ("createUser:bool?", false));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<bool> { Data = false, Succeeded = false, Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message, ex.InnerException.Message } });
            }
        }
    }
}
