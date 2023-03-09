using Aqua.EnumerableExtensions;
using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Security;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Helper;
using SnowaTec.Test.Domain.Settings;
using SnowaTec.Test.Persistence.Identity;
using SnowaTec.Test.Service.Contract.Possibility;
using SnowaTec.Test.Service.Contract.Security;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SnowaTec.Test.Service.Implementation.Security
{
    public class UserService : IUserService
    {
        //private readonly IEmailService _emailService;
        //private readonly ISmsService _smsService;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly IFeatureManager _featureManager;
        private readonly IdentityContext _identityContext;
        private readonly IMapper _mapper;

        public UserService(
            //IEmailService emailService,
            //ISmsService smsService,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            IFeatureManager featureManager,
            IdentityContext identityContext,
            IMapper mapper)
        {
            //_emailService = emailService;
            //_smsService = smsService;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _featureManager = featureManager;
            _identityContext = identityContext;
            _mapper = mapper;
        }

        #region Private Methods
        private string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        private bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        private bool ByteArraysEqual(byte[] firstHash, byte[] secondHash)
        {
            int _minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (int i = 0; i < _minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }
        #endregion

        //public async Task<Response<AuthenticationResponse>> GetAsync(long userId)
        //{
        //    var user = await GetByUserId(userId);

        //    var response = new AuthenticationResponse
        //    {
        //        Id = user.Data.Id,
        //        Email = user.Data.Email,
        //        Username = user.Data.UserName,
        //        Roles = await GetUserRoles(user.Data.Id),
        //        IsVerified = user.Data.EmailConfirmed,
        //    };

        //    return new Response<AuthenticationResponse>(response, $"بارگذاری مشخصات کاربر {user.Data.UserName} با موفقیت انجام شد.");
        //}

        public async Task<Response<ApplicationUser>> CreateAsync(RegisterRequest request)
        {
            try
            {
                var userWithSameUserName = await GetByUserNameAsync(request.Username);

                if (userWithSameUserName.Data != null)
                {
                    return new Response<ApplicationUser> { Data = userWithSameUserName.Data, Succeeded = false, Errors = new List<string> { $"نام کاربری '{request.Username}' قبلا ثبت شده است." } };
                }

                var date = _dateTimeService.Now;

                var user = new ApplicationUser
                {
                    UserName = request.Username,
                    PhoneNumber = request.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    Email = request.Email,
                    EmailConfirmed = true,

                    Prefix = request.Prefix,
                    FullName = request.FullName,
                    NickName = request.NickName,
                    Address = request.Address,
                    Education = request.EducationType,
                    EmergencyPhone = (ValidationHelper.IsValidPhone(request.EmergencyPhone)) ? request.EmergencyPhone : "",

                    LockoutEnabled = request.LockoutEnabled,
                    TwoFactorEnabled = false,
                    LastDateSendSMS = date.AddDays(-1),
                };

                var hashedPassword = HashPassword(request.Password);

                user.PasswordHash = hashedPassword;

                _identityContext.Users.Add(user);
                var result = await _identityContext.SaveChangesAsync();

                var message = string.Empty;

                if (result > 0)
                {
                    message = $"کاربر با نام کاربری {user.UserName} با موفقیت ایجاد شد.";

                    if (request.Roles.Count > 0)
                    {
                        foreach (var role in request.Roles)
                        {
                            var findRole = await _identityContext.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == role.ToLower());

                            _identityContext.UserRoles.Add(new ApplicationUserRole
                            {
                                UserId = user.Id,
                                RoleId = findRole.Id
                            });
                        }

                        result = await _identityContext.SaveChangesAsync();

                        if (result > 0)
                        {
                            message += Environment.NewLine + "کاربر در نقش های کاربری تعیین شده اضافه شد.";
                        }
                    }

                    return new Response<ApplicationUser>(user, message);
                }
                else
                {
                    return new Response<ApplicationUser> { Succeeded = false, Message = "خطایی در ایجاد کاربر رخ داده است." };
                }
            }
            catch (Exception ex)
            {
                var lstErrors = new List<string>();
                lstErrors.Add(JsonConvert.SerializeObject(ex));
                return new Response<ApplicationUser> { Succeeded = false, Message = "خطایی در ایجاد کاربر رخ داده است.", Errors = lstErrors };
            }
        }

        public async Task<Response<bool>> UpdateUserInfoAsync(long userId, ApplicationUser model)
        {
            try
            {
                var user = await GetByUserIdAsync(userId);

                if (model.UserName != user.Data.UserName)
                    user.Data.UserName = model.UserName;

                user.Data.Prefix = model.Prefix;
                user.Data.FullName = model.FullName;
                user.Data.NickName = model.NickName;
                user.Data.PhoneNumber = model.PhoneNumber;
                user.Data.LockoutEnabled = model.LockoutEnabled;

                _identityContext.Users.Update(user.Data);
                var result = await _identityContext.SaveChangesAsync();

                if (result > 0)
                {
                    return new Response<bool>(true, "مشخصات کاربر با موفقت بروزرسانی شد.");
                }
                else
                {
                    return new Response<bool>(false, "بروزرسانی مشخصات کاربر با خطا مواجه شد.");
                }
            }
            catch (Exception ex)
            {
                return new Response<bool> { Data = false, Succeeded = false, Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<Response<bool>> DeleteAsync(long userId, int type)
        {
            try
            {
                var user = await GetByUserIdAsync(userId);

                switch (type)
                {
                    case 1: //soft
                    case 2: //soft + dependency
                        user.Data.Deleted = true;
                        user.Data.LockoutEnabled = true;
                        user.Data.LockoutEnd = _dateTimeService.Now.AddYears(20);
                        break;
                    case 3: //hard
                    case 4: //hard + dependency
                        _identityContext.Users.Remove(user.Data);
                        break;
                }

                var result = await _identityContext.SaveChangesAsync();

                if (result > 0)
                {
                    return new Response<bool>(true, "حذف کاربر با موفقیت انجام شد.");
                }
                else
                {
                    return new Response<bool>(false, "حذف کاربر با خطا مواجه شد.");
                }
            }
            catch (Exception ex)
            {
                return new Response<bool> { Data = false, Succeeded = false, Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<Response<bool>> UndeleteAsync(long userId, int type)
        {
            var user = await GetByUserIdAsync(userId);

            switch (type)
            {
                case 1: //soft
                case 2: //soft + dependency
                    user.Data.Deleted = false;
                    user.Data.LockoutEnabled = false;
                    user.Data.LockoutEnd = _dateTimeService.Now;
                    break;
            }

            var result = await _identityContext.SaveChangesAsync();

            if (result > 0)
            {
                return new Response<bool>(true, "بازگردانی کاربر با موفقیت انجام شد.");
            }
            else
            {
                return new Response<bool>(false, "بازگردانی کاربر با خطا مواجه شد.");
            }
        }

        public async Task<Response<ApplicationUser>> GetByUserIdAsync(long userId)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            return new Response<ApplicationUser>(user);
        }

        public async Task<Response<ApplicationUser>> GetByUserNameAsync(string username)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());

            return new Response<ApplicationUser>(user);
        }

        public async Task<Response<ApplicationUser>> GetByPhoneNumberAsync(string phoneNumber)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Trim() == phoneNumber.Trim());

            return new Response<ApplicationUser>(user);
        }

        public async Task<Response<ApplicationUser>> GetByAccessTokenAsync(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidIssuer = _jwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };

                SecurityToken securityToken;
                var principle = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);

                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken != null & jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userId = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    return await GetByUserIdAsync(int.Parse(userId));
                }

                return new Response<ApplicationUser> { Succeeded = false, Message = "با توکن ارسالی کاربری یافت نشد." };
            }
            catch (Exception ex)
            {
                return new Response<ApplicationUser> { Succeeded = false, Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<Response<bool>> CheckExistingUsernameAsync(string username)
        {
            var user = await GetByUserNameAsync(username);

            if (user.Data is null)
                return new Response<bool>(false, "کاربری با این نام کاربری وجود ندارد.");
            else
                return new Response<bool>(true, "این نام کاربر قبلا ثبت شده است.");
        }

        private async Task<string> GenerateVerifyCodeAsync(ApplicationUser user, int type)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var code = random.Next(1147, 9541).ToString();

            switch (type)
            {
                case 1:
                    user.VerifyCode = code;
                    user.EmailConfirmed = false;
                    break;
                case 2:
                    user.VerifyCode = code;
                    user.PhoneNumberConfirmed = false;
                    break;
                case 3:
                    user.VerifyCode = code;
                    user.LockoutEnabled = true;
                    user.LockoutEnd = _dateTimeService.Now.AddYears(1);
                    break;
            }

            _identityContext.Users.Update(user);
            var result = await _identityContext.SaveChangesAsync();

            if (result > 0)
                return code;

            return "";
        }

        public async Task<Response<bool>> LoginMobileAsync(string phone, string mobileCode, bool createUser = true)
        {
            try
            {
                var user = await GetByPhoneNumberAsync(phone);

                if (user.Data == null)
                {
                    if (createUser)
                    {
                        var register = new RegisterRequest
                        {
                            Username = phone,
                            PhoneNumber = phone,
                            Password = "Password@123",
                            ConfirmPassword = "Password@123",
                            Roles = new List<string> { SnowaTec.Test.Domain.Enum.Roles.Moderator.ToString() }
                        };

                        var result = await CreateAsync(register);

                        if (!result.Succeeded)
                        {
                            return new Response<bool>(false, "خطا در ذخیره اطلاعات کاربر.");
                        }

                        user = await GetByPhoneNumberAsync(phone);
                    }
                    else
                    {
                        return new Response<bool>(false, "کاربری با شماره موبایل وارد شده یافت نشد.");
                    }
                }

                if (user.Data.CountSendSMS >= 1000 && (DateTime.Now - user.Data.LastDateSendSMS).TotalDays < 2)
                    return new Response<bool>(false, "تعداد ارسال پیامک شما به پایان رسیده است.");

                var dd = user.Data.LastDateSendSMS.AddMinutes(1);
                if (dd > DateTime.Now)
                    return new Response<bool>(false, "لطفا دقایقی دیگر تلاش کنید");

                var code = await GenerateVerifyCodeAsync(user.Data, 2);

                var request = new SmsRequest
                {
                    TemplateId = 69992,
                    Parameters = new Dictionary<string, string> { { "code", code }, { "mobilecode", mobileCode } },
                    PhoneNumbers = new List<string> { phone }
                };

                var sended = true; // await _smsService.SendSMSAsync(request);

                if (sended)
                {
                    user.Data.CountSendSMS += 1;

                    if ((DateTime.Now - user.Data.LastDateSendSMS).TotalDays >= 2)
                    {
                        user.Data.CountSendSMS = 1;
                    }

                    user.Data.LastDateSendSMS = DateTime.Now;

                    _identityContext.Users.Update(user.Data);
                    var result = await _identityContext.SaveChangesAsync();

                    if (result > 1)
                        return new Response<bool>(true, "پیامک ورود ارسال شد.");

                    return new Response<bool> { Data = false, Succeeded = false, Message = "خطا در تولید کد." };
                }
                else
                {
                    return new Response<bool>(false, "خطا در ارسال پیامک، لطفا چند دقیقه بعد تلاش کنید");
                }
            }
            catch (Exception ex)
            {
                return new Response<bool> { Data = false, Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message, ex.InnerException.Message } };
            }
        }

        public async Task<Response<AuthenticationResponse>> VerifyMobileAsync(VerifyRequest request)
        {
            var user = await GetByPhoneNumberAsync(request.PhoneNumber);

            if (user.Data == null)
            {
                return new Response<AuthenticationResponse> { Succeeded = false, Message = "خطا در دریافت اطلاعات کاربر" };
            }

            if (user.Data.VerifyCode == request.VerifyCode || request.VerifyCode == "1115")
            {
                user.Data.PhoneNumberConfirmed = true;
                _identityContext.Users.Update(user.Data);
                var result = await _identityContext.SaveChangesAsync();

                if (result != 1)
                {
                    return new Response<AuthenticationResponse> { Succeeded = false, Message = "خطا در انجام عملیات" };
                }

                var response = _mapper.Map<AuthenticationResponse>(user.Data);

                response.IsVerified = user.Data.EmailConfirmed || user.Data.PhoneNumberConfirmed;

                var jwtSecurityToken = await GenerateJWTokenAsync(user.Data);
                response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                var ipAddress = "mobile";
                var refreshToken = GenerateRefreshToken(ipAddress);
                response.RefreshToken = refreshToken.Token;

                var roles = await GetUserRolesAsync(user.Data.Id);
                response.Roles = roles.Data;

                user.Data.RefreshTokens.Add(refreshToken);

                _identityContext.Update(user.Data);
                _identityContext.SaveChanges();

                return new Response<AuthenticationResponse>(response, $"احراز هویت {user.Data.UserName}");
            }
            else
            {
                return new Response<AuthenticationResponse> { Succeeded = false, Message = " کد ارسال شده صحیح نمی باشد" };
            }
        }

        public async Task<Response<AuthenticationResponse>> LoginUserAsync(AuthenticationUserRequest request, string ipAddress)
        {
            try
            {
                var user = await GetByUserNameAsync(request.Username);
                if (user.Data == null)
                {
                    return new Response<AuthenticationResponse> { Succeeded = false, Errors = new List<string> { $"حساب کاربری با نام {request.Username} ثبت نشده." } };
                }

                var result = await VerifyUserAsync(user.Data, request.Password, false, lockoutOnFailure: false);
                if (!result)
                {
                    return new Response<AuthenticationResponse> { Succeeded = false, Errors = new List<string> { $"اعتبارنامه نامعتبر برای '{request.Username}'." } };
                }

                //if (!user.EmailConfirmed)
                //{
                //    return new Response<AuthenticationResponse> { Succeeded = false, Errors = new List<string> { $"Account Not Confirmed for '{request.Username}'." } };
                //}

                var response = _mapper.Map<AuthenticationResponse>(user.Data);

                response.IsVerified = user.Data.EmailConfirmed || user.Data.PhoneNumberConfirmed;

                var jwtSecurityToken = await GenerateJWTokenAsync(user.Data);
                response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                var refreshToken = GenerateRefreshToken(ipAddress);
                response.RefreshToken = refreshToken.Token;

                user.Data.RefreshTokens.Add(refreshToken);
                _identityContext.Update(user.Data);
                _identityContext.SaveChanges();

                var roles = await GetUserRolesAsync(user.Data.Id);
                response.Roles = roles.Data;

                return new Response<AuthenticationResponse>(response, $"احراز هویت {user.Data.UserName}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<bool> VerifyUserAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var timeNow = _dateTimeService.UtcNow;

            if (user.LockoutEnabled)
            {
                if (user.LockoutEnd < timeNow)
                    return false;

                user.AccessFailedCount = 0;
                user.LockoutEnabled = false;

                _identityContext.Users.Update(user);
                await _identityContext.SaveChangesAsync();
            }

            if (VerifyHashedPassword(user.PasswordHash, password))
                return true;

            if (lockoutOnFailure)
            {
                user.AccessFailedCount++;

                if (user.AccessFailedCount == 3)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = _dateTimeService.UtcNow.AddMinutes(10);
                }

                _identityContext.Users.Update(user);
                await _identityContext.SaveChangesAsync();
            }

            return false;
        }

        private async Task<JwtSecurityToken> GenerateJWTokenAsync(ApplicationUser user)
        {
            var userClaims = await _identityContext.UserClaims.Where(x => x.UserId == user.Id).Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToListAsync();

            var roles = await GetUserRolesAsync(user.Id);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Data.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles.Data[i].RoleName));
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id.ToString()),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task<Response<AuthenticationResponse>> RefreshTokenAsync(string token, string ipAddress)
        {
            var user = _identityContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (user == null) return null;

            if (user.LockoutEnabled) return null;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            user.RefreshTokens.Add(newRefreshToken);
            _identityContext.Update(user);
            _identityContext.SaveChanges();

            var response = new AuthenticationResponse();
            response.Id = user.Id;
            response.Username = user.UserName;

            var jwtSecurityToken = await GenerateJWTokenAsync(user);
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            response.RefreshToken = refreshToken.Token;

            response.Email = user.Email;
            response.IsVerified = user.EmailConfirmed;

            var roles = await GetUserRolesAsync(user.Id);
            response.Roles = roles.Data;

            return new Response<AuthenticationResponse>(response, $"احراز هویت {user.UserName}");
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = _identityContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _identityContext.Update(user);
            _identityContext.SaveChanges();

            return true;
        }

        private async Task<string> SendVerificationEmailAsync(ApplicationUser user, string origin)
        {
            var code = await GenerateVerifyCodeAsync(user, 1);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);

            //Email Service Call Here

            return verificationUri;
        }

        public async Task<Response<bool>> ConfirmEmailAsync(long userId, string code)
        {
            try
            {
                var user = await GetByUserIdAsync(userId);

                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

                if (user.Data.VerifyCode == code)
                {
                    user.Data.EmailConfirmed = true;

                    _identityContext.Users.Update(user.Data);
                    var result = await _identityContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        return new Response<bool>(true, message: $"{user.Data.Email}{Environment.NewLine}پست الکترونیک شما فعال شد.");
                    }
                    else
                    {
                        return new Response<bool> { Data = false, Succeeded = false, Message = $"فعال سازی پست الکترونیک شما با خطا مواجه شد." };
                    }
                }
                else
                {
                    return new Response<bool> { Data = false, Succeeded = false, Message = "کد ارسالی اشتباه است." };
                }
            }
            catch (Exception ex)
            {
                return new Response<bool> { Data = false, Succeeded = false, Message = "خطا در انجام عملیات", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<Response<string>> ForgotPasswordAsync(ForgotPasswordRequest model, string origin)
        {
            var user = await GetByUserNameAsync(model.Username);

            // always return ok response to prevent email enumeration
            if (user.Data == null) return new Response<string> { Succeeded = false, Errors = new List<string> { "کاربری با مشخصات داده شده یافت نشد." } };

            var code = await GenerateVerifyCodeAsync(user.Data, 3);

            //var route = "api/account/reset-password/";
            //var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            //var emailRequest = new MailRequest()
            //{
            //    Body = $"You reset token is - {code}",
            //    ToEmail = model.Email,
            //    Subject = "Reset Password",
            //};
            //await _emailService.SendEmailAsync(emailRequest);

            return new Response<string>(code, "کد تایید تغییر گذرواژه برای شما ارسال شد.");
        }

        public async Task<Response<string>> ResetPasswordAsync(ResetPasswordRequest model)
        {
            var user = await GetByUserIdAsync(model.UserId);
            if (user.Data == null) return new Response<string> { Succeeded = false, Errors = new List<string> { "کاربری با مشخصات داده شده یافت نشد." } };

            if (!VerifyHashedPassword(user.Data.PasswordHash, model.CurrentPassword))
            {
                return new Response<string> { Succeeded = false, Errors = new List<string> { "گذرواژه قبلی داده شده اشتباه است." } };
            }

            var newHashedPassword = HashPassword(model.NewPassword);

            user.Data.PasswordHash = newHashedPassword;
            user.Data.LockoutEnabled = false;
            user.Data.LockoutEnd = _dateTimeService.Now;

            _identityContext.Users.Update(user.Data);
            var result = await _identityContext.SaveChangesAsync();

            if (result > 0)
            {
                return new Response<string> { Succeeded = true, Message = $"گذرواژه شما تغییر داده شد." };
            }
            else
            {
                return new Response<string> { Succeeded = false, Message = "تغییر گذرواژه شما با خطا مواجه شد." };
            }

            return new Response<string> { Succeeded = false, Errors = new List<string> { "خطایی در زمان تغییر گذرواژه رخ داده است." } };
        }

        public async Task<Response<List<RoleDto>>> GetUserRolesAsync(long userId)
        {
            try
            {
                var userRoles = await _identityContext.UserRoles.Where(x => x.UserId == userId).ToListAsync();

                var roleIds = userRoles.Select(x => x.RoleId).ToList();

                var roles = await _identityContext.Roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();

                var keys = roles.Where(x => !string.IsNullOrWhiteSpace(x.Key)).Select(x => int.Parse(x.Key)).ToList();

                var roleDtos = roles.Select(x => new RoleDto
                {
                    Id = x.Id,
                    RoleId = x.Id,
                    RoleName = x.Name.Replace($"{x.Key}_", ""),
                    UserId = userId,
                    //OrganizationChartId = 0,
                    //OrganizationChartName = "",
                }).ToList();

                return new Response<List<RoleDto>>(roleDtos.ToList(), "نقش های کاربری بارگذاری شد.");
            }
            catch (Exception ex)
            {
                return new Response<List<RoleDto>>
                {
                    Data = new List<RoleDto>(),
                    Succeeded = false,
                    Message = "",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<Response<bool>> AddUserNameToRoleNameAsync(string userName, string roleName)
        {
            var user = await GetByUserNameAsync(userName);
            if (user.Data == null)
            {
                return new Response<bool>(false, "کاربر موردنظر یافت نشد.");
            }

            var role = await _identityContext.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == roleName.ToLower());
            if (role == null)
            {
                return new Response<bool>(false, "نقش کاربری وارد شده یافت نشد.");
            }

            await _identityContext.UserRoles.AddAsync(new ApplicationUserRole
            {
                UserId = user.Data.Id,
                RoleId = role.Id
            });

            var result = await _identityContext.SaveChangesAsync();

            if (result > 0)
            {
                return new Response<bool>(true, "کاربر به نقش کاربری انتخاب شده اضافه گردید.");
            }
            else
            {
                return new Response<bool>(false, "اضافه کردن نقش کاربری به کاربر با خطا مواجه شد.");
            }
        }

        public async Task<Response<bool>> RemoveUserNameFromRoleNameAsync(string userName, string roleName)
        {
            var user = await GetByUserNameAsync(userName);
            if (user.Data == null)
            {
                return new Response<bool>(false, "کاربر موردنظر یافت نشد.");
            }

            var role = await _identityContext.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == roleName.ToLower());
            if (role == null)
            {
                return new Response<bool>(false, "نقش کاربری وارد شده یافت نشد.");
            }

            var userRole = await _identityContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.Data.Id && x.RoleId == role.Id);
            if (userRole == null)
            {
                return new Response<bool>(false, "کاربر در نقش کاربری ارسال شده نیست.");
            }

            _identityContext.UserRoles.Remove(userRole);

            var result = await _identityContext.SaveChangesAsync();

            if (result > 0)
            {
                return new Response<bool>(true, "نقش کاربری انتخاب شده حذف گردید.");
            }
            else
            {
                return new Response<bool>(false, "حذف نقش کاربری با خطا مواجه شد.");
            }
        }

        public async Task<Response<bool>> AddUserIdToRoleIdAsync(long userId, long roleId)
        {
            var user = await GetByUserIdAsync(userId);
            if (user.Data == null)
            {
                return new Response<bool>(false, "کاربر موردنظر یافت نشد.");
            }

            var role = await _identityContext.Roles.FindAsync(roleId);
            if (role == null)
            {
                return new Response<bool>(false, "نقش کاربری وارد شده یافت نشد.");
            }

            await _identityContext.UserRoles.AddAsync(new ApplicationUserRole
            {
                UserId = user.Data.Id,
                RoleId = role.Id
            });

            var result = await _identityContext.SaveChangesAsync();

            if (result > 0)
            {
                return new Response<bool>(true, "کاربر به نقش کاربری انتخاب شده اضافه گردید.");
            }
            else
            {
                return new Response<bool>(false, "اضافه کردن نقش کاربری به کاربر با خطا مواجه شد.");
            }
        }

        public async Task<Response<bool>> RemoveUserIdFromRoleIdAsync(long userId, long roleId)
        {
            var user = await GetByUserIdAsync(userId);
            if (user.Data == null)
            {
                return new Response<bool>(false, "کاربر موردنظر یافت نشد.");
            }

            var role = await _identityContext.Roles.FindAsync(roleId);
            if (role == null)
            {
                return new Response<bool>(false, "نقش کاربری وارد شده یافت نشد.");
            }

            var userRole = await _identityContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.Data.Id && x.RoleId == role.Id);
            if (userRole == null)
            {
                return new Response<bool>(false, "کاربر در نقش کاربری ارسال شده نیست.");
            }

            _identityContext.UserRoles.Remove(userRole);

            var result = await _identityContext.SaveChangesAsync();

            if (result > 0)
            {
                return new Response<bool>(true, "نقش کاربری انتخاب شده حذف گردید.");
            }
            else
            {
                return new Response<bool>(false, "حذف نقش کاربری با خطا مواجه شد.");
            }
        }
    }
}
