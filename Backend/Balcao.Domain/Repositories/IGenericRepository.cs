namespace Balcao.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query();
        T Get(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
