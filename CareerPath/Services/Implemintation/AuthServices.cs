using CareerPath.Contracts.Requests;
using CareerPath.Contracts.Responses;
using CareerPath.Helper;
using CareerPath.Models;
using CareerPath.Services.Abstraction;
using CareerPath.Templets;
using FluentResults;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace CareerPath.Services.Implemintation
{
    public class AuthServices(
        UserManager<UserApp> userManager, ITokenHelper tokenHelper, IHttpContextAccessor accessor,
        IEmailServices emailServices, IConfiguration configuration) : IAuthServices
    {
        private readonly UserManager<UserApp> _userManager = userManager;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly IHttpContextAccessor _contextAccessor = accessor;
        private readonly IEmailServices _emailServices = emailServices;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Result<LoginResponse>> Login(LoginRequest loginRequest)
        {

            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
                return Result.Fail(new Error("Invalid Email Or Password"));

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!isPasswordValid)
                return Result.Fail(new Error("Invalid Email Or Password"));

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (role is null) return Result.Fail(new Error("Invalid Email Or Password"));

            var (token, expiresIn) = _tokenHelper.GenerateToken(user, role!);
            var refreshToken = RefreshToken();
            var ExpireDate = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("JWT:RefreshTokenExpiresInDays"));
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = ExpireDate
            });
            await _userManager.UpdateAsync(user);
            return new LoginResponse(user.Id, user.Email, user.UserName!, token, expiresIn, refreshToken, ExpireDate);
        }
        private static string RefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        public async Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var userId = _tokenHelper.ValidateToken(request.Token);
            if (userId is null)
                return Result.Fail(new Error("Invalid Token"));
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Fail(new Error("Not Found"));
            var userRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == request.RefreshToken && rt.IsActive);
            if (userRefreshToken is null)
                return Result.Fail(new Error("Invalid Refresh Token"));
            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (role is null) return Result.Fail(new Error("not found"));

            var (NewToken, expiresIn) = _tokenHelper.GenerateToken(user, role!);
            var NewRefreshToken = RefreshToken();
            var ExpireDate = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("JWT:RefreshTokenExpiresInDays"));
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = NewRefreshToken,
                ExpiresOn = ExpireDate
            });
            await _userManager.UpdateAsync(user);
            return new LoginResponse(user.Id, user.Email, user.UserName!, NewToken, expiresIn, NewRefreshToken, ExpireDate);
        }
        public async Task<Result> RegesterAsync(RegesterRequest request)
        {
            var oldUser = await _userManager.FindByEmailAsync(request.Email);
            if (oldUser is not null)
                return Result.Fail("Email is already in use.");
            var newUser = request.Adapt<UserApp>();
            newUser.UserName = $"{request.FirstName}{request.LastName}".Trim();
            var create = await _userManager.CreateAsync(newUser, request.Password);
            if (!create.Succeeded)
                return Result.Fail("Failed to create user.");
            await SendConfirmationEmailAsync(newUser);
            await _userManager.AddToRoleAsync(newUser, "User");
            return Result.Ok();
        }

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Result.Fail("Invalid User");

            if (user.EmailConfirmed)
                return Result.Fail("Email is already confirmed");
            var decodedToken = request.Token;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            }
            catch
            {
                return Result.Fail("Invalid Token");
            }

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
                return Result.Fail("Email Confirmation Failed");

            return Result.Ok();
        }

        public async Task<Result> LogoutAsync(LogoutRequest request)
        {
            var userId = _tokenHelper.ValidateToken(request.Token);
            if (userId is null)
                return Result.Fail(new Error("Invalid Token"));
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Fail(new Error("Not Found"));

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == request.RefreshToken && rt.IsActive);
            if (userRefreshToken is null)
                return Result.Fail(new Error("Invalid Refresh Token"));
            userRefreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return Result.Ok();
        }

        public async Task SendConfirmationEmailAsync(UserApp user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var prarm = new Dictionary<string, string?>
            {
               { "Token", encodedToken },
               { "UserId", user.Id },
               { "Email", user.Email }
            };

            var origin = _contextAccessor.HttpContext?.Request.Headers.Origin;
            var baseUrl = $"{origin}/auth/email-confirm";
            var callBack = QueryHelpers.AddQueryString(baseUrl, prarm);
            var emailBody = EmailTemplet.ConfirmEmailTemplate(user.UserName, callBack);

            await _emailServices.sendEmailAsync(
             user.Email,
             "Confirm Your Email - CareerPath",
             emailBody
              );
        }

        public async Task<Result> ReSendConfirmationEmailAsync(ReSendConfirmationEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Fail("not Found");
            if (user.EmailConfirmed)
                return Result.Fail("Email already confirmed");

            await SendConfirmationEmailAsync(user);
            return Result.Ok();
        }

        public async Task SendConfirmForgetPasswordEmailAsync(UserApp user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var prarm = new Dictionary<string, string?>
             {
               { "Token", encodedToken },
               { "UserId", user.Id },
               { "Email", user.Email }
             };

            var origin = _contextAccessor.HttpContext?.Request.Headers.Origin;
            var baseUrl = $"{origin}/auth/reset-password";
            var callBack = QueryHelpers.AddQueryString(baseUrl, prarm);

            var emailBody = EmailTemplet.ResetPasswordTemplate(user.UserName, callBack);

            await _emailServices.sendEmailAsync(
                user.Email,
                "Reset Your Password - CareerPath",
                emailBody
            );
        }

        public async Task<Result> SendResetPasswordTokenAsync(SendResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Fail("not Found");
            if (!user.EmailConfirmed)
                return Result.Fail("Email Not Confirmed");
            await SendConfirmForgetPasswordEmailAsync(user);
            return Result.Ok();
        }


        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Fail("Not Found");
            var decodedToken = request.Token;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            }
            catch
            {
                return Result.Fail("Invalid Token");
            }
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            if (!result.Succeeded)
                return Result.Fail("Password Reset Failed");
            return Result.Ok();
        }

    }
}
