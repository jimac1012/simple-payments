using Model;

namespace Application.Interfaces
{
    public interface IAccountLogic
    {
        AccountModel GetUserAccount(int id);

        void Save(AccountModel model);

        void Dispose();
    }
}
