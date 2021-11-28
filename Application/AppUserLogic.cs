using Application.Interfaces;
using Domain;
using Model;
using Repository.Interfaces;
using System;
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

        public void Save(AppUserModel userModel)
        {
            if (Repository.GetAll().Any(x => x.EmailAddress == userModel.EmailAddress))
                throw new Exception("Email already used.");

            AppUser appUser = new AppUser() { 
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                EmailAddress= userModel.EmailAddress
            };
            Repository.Add(appUser);
            UnitOfWork.Save();
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }

        public AppUserModel Get(int id)
        {
            AppUser appUser = Repository.GetAll().FirstOrDefault(x => x.Id == id);

            if (appUser == null)
                return null;
            
            return CreateUserModel(appUser);
        }

        public AppUserModel GetByEmailAddress(string emailAddress)
        {
            AppUser appUser = Repository.GetAll().FirstOrDefault(x => x.EmailAddress == emailAddress);

            if (appUser == null)
                return null;

            return CreateUserModel(appUser);
        }

        private AppUserModel CreateUserModel(AppUser appUser)
        {
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
