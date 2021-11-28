using System;
using Domain;

namespace Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        PaymentContext DbContext { get; }

        int Save();
    }
}
