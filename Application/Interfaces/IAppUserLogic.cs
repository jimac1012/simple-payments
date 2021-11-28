using Model;

namespace Application.Interfaces
{
    public interface IAppUserLogic
    {
        AppUserModel GetByEmailAddress(string emailAddress);

        TransactionStatus Save(AppUserModel userModel);

        void Dispose();
    }
}
