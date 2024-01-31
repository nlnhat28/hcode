using HCode.Domain;
using System.Security.Claims;

namespace HCode.Application
{
    /// <summary>
    /// Service auth
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public interface IAuthService : ICoreService
    {
        /// <summary>
        /// Tạo mới tài khoản
        /// </summary>
        /// <returns></returns>
        Task SignupAsync(AuthDto authDto, ServerResponse res);
        /// <summary>
        /// Xác thực tài khoản
        /// </summary>
        /// <returns></returns>
        Task VerifyAsync(AuthDto authDto, ServerResponse res);
        /// <summary>
        /// Gửi email xác thực
        /// </summary>
        /// <returns>Mã xác thực</returns>
        Task<string?> SendVerifyCodeAsync(AuthDto authDto, ServerResponse res);
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <returns></returns>
        Task LoginAsync(AuthDto authDto, ServerResponse res);
        /// <summary>
        /// Quên mật khẩu
        /// </summary>
        /// <returns></returns>
        Task ForgotPasswordAsync(AuthDto authDto, ServerResponse res);
        /// <summary>
        /// Lấy AccountId hiện tại
        /// </summary>
        /// <returns></returns>
        Guid GetAccountId();
        /// <summary>
        /// Lấy Role hiện tại
        /// </summary>
        /// <returns></returns>
        RoleCode? GetRole()
        /// <summary>
        /// Lấy AccountId.ToString() hiện tại
        /// </summary>
        /// <returns></returns>
        string GetAccountIdToString();
        /// <summary>
        /// Lấy AccessToken
        /// </summary>
        /// <returns></returns>
        string? GetToken();
        /// <summary>
        /// Lấy Claim theo key
        /// </summary>
        /// <returns></returns>
        Claim? GetClaim(string keyClaim);
    }
}
