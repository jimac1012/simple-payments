using Model;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ITransactionLogic
    {
        ITransactionStatus Credit(TransactionalModel model);

        ITransactionStatus Debit(TransactionalModel model);

        IList<TransactionModel> GetAllPerAccount(GetAllAccountTransactionsModel model);

        void Dispose();
    }
}
