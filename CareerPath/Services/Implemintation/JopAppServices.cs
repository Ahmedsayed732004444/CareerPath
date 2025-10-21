using CareerPath.Contracts.Responses;
using CareerPath.Services.Abstraction;
using CareerPath.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CareerPath.Services.Implemintation
{
    public class JopAppServices(IUnitWork _unitWork) : IJopAppServices
    {
        public async Task<IEnumerable<GetAllJopAppResponse?>> GetAllJopAppAsync()
        {
            return await _unitWork.JopAppRepository.GetAll().AsNoTracking().Select(J => new GetAllJopAppResponse
            (
                 J.ID,
                 J.Titel
            )).ToListAsync();
        }
    }
}
