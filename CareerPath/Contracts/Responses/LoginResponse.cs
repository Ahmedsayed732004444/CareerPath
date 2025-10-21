namespace CareerPath.Contracts.Responses
{
    public record LoginResponse
    (
        string id,
        string? Email,
        string UserName,
        string Token,
        int ExpiresIn,
        string refreshToken,
        DateTime refreshTokenExpiration
    );
}
