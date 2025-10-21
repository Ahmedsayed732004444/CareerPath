namespace CareerPath.Contracts.Requests
{
    public record RegesterRequest
    (
        string FirstName,
        string LastName,
        string Email,
        string Password
    );
}
