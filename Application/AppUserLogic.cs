using Application.Interfaces;
using Domain;
using Model;
using Repository.Interfaces;
using System.Linq;

namespace Application
{
    public class AppUserLogic : IAppUserLogic
    {
        private IUnitOfWork UnitOfWork { get; }
        
        private IGenericRepository<AppUser> Repository { get; }

        public AppUserLogic(IUnitOfWork unitOfWork, IGenericRepository<AppUser> genericRepository)
        {
            UnitOfWork = unitOfWork;
            Repository = genericRepository;
        }

        public ITransactionStatus Save(AppUserModel userModel)
        {
            var result = new TransactionStatus();

            if (Repository.GetAll().Any(x => x.EmailAddress == userModel.EmailAddress))
            {
                result.UpdateMessage("Email already used.");
            }
            else
            {
                AppUser appUser = new AppUser()
                {
                    Id = userModel.Id,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    EmailAddress = userModel.EmailAddress
                };
                Repository.Add(appUser);
                UnitOfWork.Save();
                result.TransactionSuccess();
            }

            return result;
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }

        public AppUserModel GetByEmailAddress(string emailAddress)
        {
            AppUser appUser = Repository.GetAll()
                .FirstOrDefault(x => x.EmailAddress == emailAddress);

            if (appUser == null)
                return null;


            return new AppUserModel()
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                EmailAddress = appUser.EmailAddress
            };
        }
    }
}
