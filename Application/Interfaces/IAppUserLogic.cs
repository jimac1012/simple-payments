using Model;

namespace Application.Interfaces
{
    public interface IAppUserLogic
    {
        AppUserModel GetByEmailAddress(string emailAddress);

        ITransactionStatus Save(AppUserModel userModel);

        void Dispose();
    }
}
