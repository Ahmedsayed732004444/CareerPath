namespace CareerPath.Repository.GenaricRepository
{
    public interface IGenaricRepo<TEntity>
    {
        public IQueryable<TEntity> GetAll();
    }
}
