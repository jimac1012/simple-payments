using System.Linq;

namespace Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
