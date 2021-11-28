using Domain;
using Repository.Interfaces;
using System;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private PaymentContext _context;

        public UnitOfWork(PaymentContext context)
        {
            _context = context;
            _context.Configuration.LazyLoadingEnabled = false;
        }

        public PaymentContext DbContext { get { return _context; } }

        public int Save()
        {
            return _context.SaveChanges();
        }
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
