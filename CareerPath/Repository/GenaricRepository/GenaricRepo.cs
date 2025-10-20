using CareerPath.Data;

namespace CareerPath.Repository.GenaricRepository
{
    public class GenaricRepo<TEntity>(MyContext _context) : IGenaricRepo<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }
    }
}
