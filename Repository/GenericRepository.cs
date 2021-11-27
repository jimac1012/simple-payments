using Domain;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected PaymentContext _context;

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            _context = unitOfWork.DbContext;
        }

        IQueryable<T> IGenericRepository<T>.GetAll()
        {
            IQueryable<T> query = _context.Set<T>();
            return query;
        }

        void IGenericRepository<T>.Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        void IGenericRepository<T>.Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        void IGenericRepository<T>.Update(T entity)
        {
            _context.Set<T>().Attach(entity);
        }
    }
}
