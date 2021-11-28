using Application.Interfaces;
using Domain;
using Model;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Application
{
    public class AccountLogic : IAccountLogic
    {
        private IUnitOfWork UnitOfWork { get; }

        private IGenericRepository<Account> Repository { get; }

        public AccountLogic(IUnitOfWork unitOfWork, IGenericRepository<Account> genericRepository)
        {
            UnitOfWork = unitOfWork;
            Repository = genericRepository;
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }

        public IEnumerable<AccountModel> GetAllUserAccounts(int userId)
        {
            return Repository.GetAll()
                .Where(x => x.UserId == userId)
                .Select(x => new AccountModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Balance = x.Balance,
                        Type = x.Type,
                        UserId = userId
                    }
                ).ToList();
        }

        public AccountModel GetUserAccount(int id)
        {
            Account account = Repository.GetAll().FirstOrDefault(x => x.Id == id);

            if (account == null)
                return null;

            return new AccountModel()
            {
                Id = account.Id,
                Name = account.Name,
                Balance = account.Balance,
                Type = account.Type,
                UserId = account.UserId
            };
        }

        public void Save(AccountModel model)
        {
            Account account = new Account()
            {
                UserId = model.UserId,
                Name = model.Name,
                Balance = model.Balance,
                Type = model.Type
            };
            Repository.Add(account);
            UnitOfWork.Save();
        }
    }
}
