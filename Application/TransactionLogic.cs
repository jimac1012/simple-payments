using Application.Interfaces;
using Domain;
using Model;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application
{
    public class TransactionLogic : ITransactionLogic
    {
        private IUnitOfWork UnitOfWork { get; }

        private IGenericRepository<Transaction> TransactionRepository { get; }

        private IGenericRepository<Account> AccountRepository { get; }


        public TransactionLogic(IUnitOfWork unitOfWork, 
            IGenericRepository<Transaction> transactionRepository, 
            IGenericRepository<Account> accountRepository)
        {
            UnitOfWork = unitOfWork;
            TransactionRepository = transactionRepository;
            AccountRepository = accountRepository;
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }

        private Transaction CreateTransactionModel(TransactionalModel model, TransactionType type = TransactionType.Credit)
        {
            var transaction = new Transaction()
            {
                Amount = model.Amount,
                AccountId = model.AccountId,
                Status = "Closed",
                TransactionFee = model.TransactionFee,
                TransactionType = type.ToString(),
                Note = model.Note
            };

            return transaction;
        }

        public ITransactionStatus Credit(TransactionalModel model)
        {
            var result = new TransactionStatus();

            try
            {
                var account = GetAccount(model.UserId, model.AccountId);

                if (model.Amount <= 0)
                {
                    result.UpdateMessage("Transaction Invalid. " +
                        "Amount Cannot Be Greater Than Or Equal To Zero (0).");
                    return result;
                }

                var transaction = CreateTransactionModel(model, TransactionType.Credit);
                TransactionRepository.Add(transaction);

                account.AddBalance(transaction.Amount);
                AccountRepository.Update(account);
                UnitOfWork.Save();
                result.TransactionSuccess();
            }
            catch (Exception ex)
            {
                result.UpdateMessage(ex.Message);
            }

            return result;
        }

        public ITransactionStatus Debit(TransactionalModel model)
        {
            var result = new TransactionStatus();

            try
            {
                var account = GetAccount(model.UserId, model.AccountId);

                if (model.Amount <= 0)
                    result.UpdateMessage("Transaction Invalid. " +
                        "Amount Cannot Be Greater Than Or Equal To Zero (0).");
                else if (account.Balance < model.Amount + model.TransactionFee)
                    result.UpdateMessage("Transaction Invalid. Transaction Amount Greater Than Balance.");
                else
                {
                    var transaction = CreateTransactionModel(model, TransactionType.Debit);
                    TransactionRepository.Add(transaction);

                    account.DeductBalance(transaction.Amount + transaction.TransactionFee ?? 0);
                    AccountRepository.Update(account);
                    UnitOfWork.Save();
                    result.TransactionSuccess();
                }
            }
            catch (Exception ex)
            {
                result.UpdateMessage(ex.Message);
            }

            return result;
        }

        public IList<TransactionModel> GetAllPerAccount(GetAllAccountTransactionsModel model)
        {
            try
            {
                var account = GetAccount(model.UserId, model.AccountId);

                return TransactionRepository.GetAll()
                    .Where(x => x.AccountId == account.Id)
                    .OrderByDescending(x => x.TransactionDate)
                    .Select(x => new TransactionModel()
                    {
                        TransactionDate = x.TransactionDate,
                        Amount = x.Amount,
                        Status = x.Status,
                        TransactionFee = x.TransactionFee,
                        TransactionType = x.TransactionType,
                        Note = x.Note,
                        Id = x.Id
                    }
                ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Account GetAccount(int userId, int accountId)
        {
            try
            {
                var account = AccountRepository.GetAll()
                        .FirstOrDefault(x => x.Id == accountId && x.UserId == userId);

                if (account == null)
                    throw new Exception("Account Does Not Exists.");

                return account;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
