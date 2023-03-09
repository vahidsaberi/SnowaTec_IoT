using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Security;
using SnowaTec.Test.Domain.Entities.Security;

namespace SnowaTec.Test.Service.Contract.Security
{
    public interface IUserService
    {
        //Task<Response<AuthenticationResponse>> GetAsync(long userId);

        Task<Response<ApplicationUser>> CreateAsync(RegisterRequest request);

        Task<Response<bool>> UpdateUserInfoAsync(long userId, ApplicationUser model);

        Task<Response<bool>> DeleteAsync(long userId, int type);

        Task<Response<bool>> UndeleteAsync(long userId, int type);

        Task<Response<ApplicationUser>> GetByUserIdAsync(long userId);

        Task<Response<ApplicationUser>> GetByUserNameAsync(string username);

        Task<Response<ApplicationUser>> GetByPhoneNumberAsync(string phoneNumber);

        Task<Response<ApplicationUser>> GetByAccessTokenAsync(string accessToken);

        Task<Response<bool>> CheckExistingUsernameAsync(string username);

        Task<Response<bool>> LoginMobileAsync(string phone, string mobileCode, bool createUser = true);

        Task<Response<AuthenticationResponse>> VerifyMobileAsync(VerifyRequest request);

        Task<Response<AuthenticationResponse>> LoginUserAsync(AuthenticationUserRequest request, string ipAddress);

        Task<Response<AuthenticationResponse>> RefreshTokenAsync(string token, string ipAddress);

        bool RevokeToken(string token, string ipAddress);

        Task<Response<bool>> ConfirmEmailAsync(long userId, string code);

        Task<Response<string>> ForgotPasswordAsync(ForgotPasswordRequest model, string origin);

        Task<Response<string>> ResetPasswordAsync(ResetPasswordRequest model);

        Task<Response<List<RoleDto>>> GetUserRolesAsync(long userId);

        Task<Response<bool>> AddUserNameToRoleNameAsync(string userName, string roleName);

        Task<Response<bool>> RemoveUserNameFromRoleNameAsync(string userName, string roleName);

        Task<Response<bool>> AddUserIdToRoleIdAsync(long userId, long roleId);

        Task<Response<bool>> RemoveUserIdFromRoleIdAsync(long userId, long roleId);

    }
}
