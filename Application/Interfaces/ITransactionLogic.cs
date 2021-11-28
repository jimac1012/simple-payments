using Model;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ITransactionLogic
    {
        TransactionStatus Credit(TransactionalModel model);

        TransactionStatus Debit(TransactionalModel model);

        List<TransactionModel> GetAllPerAccount(int userId, int accountId);

        void Dispose();
    }
}
