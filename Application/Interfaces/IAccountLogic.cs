using Model;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IAccountLogic
    {
        AccountModel GetAccount(int id);

        IList<AccountModel> GetUserAccounts(int userId);

        ITransactionStatus Save(AccountModel model);

        void Dispose();
    }
}
