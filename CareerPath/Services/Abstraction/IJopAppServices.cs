using CareerPath.Contracts.Responses;

namespace CareerPath.Services.Abstraction
{
    public interface IJopAppServices
    {
        Task<IEnumerable<GetAllJopAppResponse?>> GetAllJopAppAsync();
    }
}
