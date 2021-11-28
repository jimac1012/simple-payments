using Application.Interfaces;
using Domain;
using Model;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class AppUserLogic : IAppUserLogic
    {
        private IUnitOfWork UnitOfWork { get; }
        
        private IGenericRepository<AppUser> UserRepository { get; }

        public AppUserLogic(IUnitOfWork unitOfWork, IGenericRepository<AppUser> genericRepository)
        {
            UnitOfWork = unitOfWork;
            UserRepository = genericRepository;
        }

        public List<AppUserModel> GetData()
        {
            List<AppUser> appUsers = UserRepository.GetAll().ToList();
            List<AppUserModel> userModels =  appUsers.Select(x => 
                new AppUserModel() { 
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    EmailAddress = x.EmailAddress
                }).ToList();

            return userModels;
        }

        public void SaveData(AppUserModel userModel)
        {
            AppUser appUser = new AppUser() { 
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                EmailAddress= userModel.EmailAddress
            };
            UserRepository.Add(appUser);
            UnitOfWork.Save();
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
