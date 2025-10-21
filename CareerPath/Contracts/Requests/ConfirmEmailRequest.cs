namespace CareerPath.Contracts.Requests
{
    public record ConfirmEmailRequest(string UserId, string Token);
}
