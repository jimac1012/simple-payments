using Application.Interfaces;
using Domain;
using Model;
using Repository.Interfaces;
using System;
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
                        AccountName = x.Name,
                        Balance = x.Balance,
                        Type = x.Type,
                        UserId = userId
                    }
                ).ToList();
        }

        public IList<AccountModel> GetUserAccounts(int userId)
        {
            if (!Repository.GetAll().Any(x => x.UserId == userId))
                throw new Exception("User does not exists.");

            var userAccounts = Repository.GetAll()
                .Where(x => x.UserId == userId)
                .Select(x => new AccountModel()
                {
                    Id = x.Id,
                    AccountName = x.Name,
                    Balance = x.Balance,
                    Type = x.Type,
                    UserId = x.UserId
                }).ToList();

            return userAccounts;
        }

        public AccountModel GetAccount(int id)
        {
            Account account = Repository.GetAll().FirstOrDefault(x => x.Id == id);

            if (account == null)
                return null;

            return new AccountModel()
            {
                Id = account.Id,
                AccountName = account.Name,
                Balance = account.Balance,
                Type = account.Type,
                UserId = account.UserId
            };
        }

        public AccountModel GetUserAccountByAccountName(int userId, string name)
        {
            Account account = Repository.GetAll()
                .FirstOrDefault(x => x.UserId == userId && x.Name == name.ToUpper());

            if (account == null)
                return null;

            return new AccountModel()
            {
                Id = account.Id,
                AccountName = account.Name,
                Balance = account.Balance,
                Type = account.Type,
                UserId = account.UserId
            };
        }

        public ITransactionStatus Save(AccountModel model)
        {
            var result = new TransactionStatus();
            
            if(Repository.GetAll()
                .Any(x => x.UserId == model.UserId && x.Name == model.AccountName))
            {
                result.UpdateMessage("Account already existing for user.");
            }
            else
            {
                Account account = new Account()
                {
                    Name = model.AccountName.ToUpper(),
                    Type = model.Type,
                    UserId = model.UserId
                };

                try
                {
                    Repository.Add(account);
                    UnitOfWork.Save();
                    result.TransactionSuccess();
                }
                catch (Exception ex)
                {
                    result.UpdateMessage(ex.Message);
                    throw;
                }
            }            

            return result;
        }
    }
}
