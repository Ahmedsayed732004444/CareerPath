using CareerPath.Contracts.Requests;
using CareerPath.Contracts.Responses;
using FluentResults;

namespace CareerPath.Services.Abstraction
{
    public interface IAuthServices
    {
        Task<Result<LoginResponse>> Login(LoginRequest loginRequest);
        Task<Result> RegesterAsync(RegesterRequest request);
        Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<Result> ReSendConfirmationEmailAsync(ReSendConfirmationEmailRequest request);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<Result> LogoutAsync(LogoutRequest request);
        Task<Result> SendResetPasswordTokenAsync(SendResetPasswordRequest request);
        Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
