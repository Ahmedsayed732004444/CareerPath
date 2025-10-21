namespace CareerPath.Contracts.Requests
{
    public record ResetPasswordRequest(string Token, string Email, string NewPassword);

}
