using CareerPath.Models;
using CareerPath.Repository.GenaricRepository;

namespace CareerPath.UnitOfWork
{
    public interface IUnitWork
    {
        IGenaricRepo<JopApp> JopAppRepository { get; }
    }
}
