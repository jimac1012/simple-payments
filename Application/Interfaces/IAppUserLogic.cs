using Model;

namespace Application.Interfaces
{
    public interface IAppUserLogic
    {
        AppUserModel GetByEmailAddress(string emailAddress);

        AppUserModel Get(int id);

        void Save(AppUserModel userModel);

        void Dispose();
    }
}
