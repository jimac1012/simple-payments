using Model;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ITransactionLogic
    {
        ITransactionStatus Credit(TransactionalModel model);

        ITransactionStatus Debit(TransactionalModel model);

        List<TransactionModel> GetAllPerAccount(int userId, int accountId);

        void Dispose();
    }
}
