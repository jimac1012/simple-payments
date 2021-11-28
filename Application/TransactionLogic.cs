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
    public class TransactionLogic : ITransactionLogic
    {
        private IUnitOfWork UnitOfWork { get; }

        private IGenericRepository<Transaction> TransactionRepository { get; }

        private IGenericRepository<Account> AccountRepository { get; }


        public TransactionLogic(IUnitOfWork unitOfWork, IGenericRepository<Transaction> transactionRepository, IGenericRepository<Account> accountRepository)
        {
            UnitOfWork = unitOfWork;
            TransactionRepository = transactionRepository;
            AccountRepository = accountRepository;
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }

        private Transaction SaveTransaction(TransactionalModel model)
        {
            var transaction = new Transaction()
            {
                Amount = model.Amount,
                AccountId = model.AccountId,
                Status = "Closed"
            };

            //try
            //{
            //    TransactionRepository.Add(transaction);
            //    UnitOfWork.Save();
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

            return transaction;
        }

        public TransactionStatus Credit(TransactionalModel model)
        {
            var result = new TransactionStatus();

            var account = GetAccount(model.UserId, model.AccountId);

            if (model.Amount <= 0)
            {
                result.UpdateMessage("Transaction Invalid. Amount Cannot Be Greater Than Or Equal To Zero (0).");
                return result;
            }

            try
            {
                var transaction = SaveTransaction(model);
                TransactionRepository.Add(transaction);

                account.Balance += transaction.Amount;
                AccountRepository.Update(account);
                UnitOfWork.Save();
                result.TransactionSuccess();
            }
            catch (Exception ex)
            {
                result.UpdateMessage(ex.Message);
                throw;
            }

            return result;
        }

        public TransactionStatus Debit(TransactionalModel model)
        {
            var result = new TransactionStatus();

            var account = GetAccount(model.UserId, model.AccountId);

            if (model.Amount <= 0)
                result.UpdateMessage("Transaction Invalid. Amount Cannot Be Greater Than Or Equal To Zero (0).");
            else if (account.Balance < model.Amount)
                result.UpdateMessage("Transaction Invalid. Transaction Amount Greater Than Balance.");
            else
            {
                try
                {
                    var transaction = SaveTransaction(model);
                    TransactionRepository.Add(transaction);

                    account.Balance -= transaction.Amount;
                    AccountRepository.Update(account);
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

        public List<TransactionModel> GetAllPerAccount(int userId, int accountId)
        {
            var account = GetAccount(userId, accountId);

            return TransactionRepository.GetAll()
                .Where(x => x.AccountId == accountId)
                .OrderByDescending(x => x.TransactionDate)
                .Select(x => new TransactionModel()
                {
                    TransactionDate = x.TransactionDate,
                    Amount = x.Amount,
                    Status = x.Status,
                    TransactionFee = x.TransactionFee,
                    Note = x.Note
                }
            ).ToList();
        }

        private Account GetAccount(int userId, int accountId)
        {
            var account = AccountRepository.GetAll()
                .FirstOrDefault(x => x.Id == accountId && x.UserId == userId);

            if (account == null)
                throw new Exception("Account Does Not Exists.");

            return account;
        }
    }
}
