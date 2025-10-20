using CareerPath.Data;
using CareerPath.Models;
using CareerPath.Repository.GenaricRepository;

namespace CareerPath.UnitOfWork
{
    public class UnitWork(MyContext _context) : IUnitWork
    {
        private readonly IGenaricRepo<JopApp> _jopAppRepository;


        public IGenaricRepo<JopApp> JopAppRepository
        {
            get
            {
                if (_jopAppRepository is null)
                {
                    return new GenaricRepo<JopApp>(_context);
                }
                return _jopAppRepository;
            }
        }
    }
}


