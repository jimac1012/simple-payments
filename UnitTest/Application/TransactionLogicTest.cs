using Application;
using Domain;
using Moq;
using System.Linq;
using Repository.Interfaces;
using System.Collections.Generic;
using Xunit;
using Model;
using System;

namespace UnitTest.Application
{
    public class TransactionLogicTest
    {
        private Mock<PaymentContext> MockContext { get; }
        private Mock<IUnitOfWork> MockUnitOfWork { get; }
        private Mock<IGenericRepository<Transaction>> MockTransactionRepo { get; }
        private Mock<IGenericRepository<Account>> MockAccountRepo { get; }

        public TransactionLogicTest()
        {
            MockUnitOfWork = new Mock<IUnitOfWork>();
            MockTransactionRepo = new Mock<IGenericRepository<Transaction>>();
            MockAccountRepo = new Mock<IGenericRepository<Account>>();
            MockContext = new Mock<PaymentContext>();
        }

        #region Credit Test

        [Fact]
        public void Transaction_Credit_Fail_UnknownIds()
        {
            // Arrange
            var user = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user.Id
            };

            var accounts = new List<Account>() { account }.AsQueryable();

            var model = new TransactionalModel()
            {
                UserId = 2,
                AccountId = 2,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Credit(model);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public void Transaction_Credit_Fail_UnknownAccount()
        {
            // Arrange
            var user = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user.Id
            };

            var accounts = new List<Account>() { account }.AsQueryable();

            var model = new TransactionalModel()
            {
                UserId = user.Id,
                AccountId = 2,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Credit(model);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public void Transaction_Credit_Fail_UnknownUser()
        {
            // Arrange
            var user = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user.Id
            };

            var accounts = new List<Account>() { account }.AsQueryable();

            var model = new TransactionalModel()
            {
                UserId = 2,
                AccountId = account.Id,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Credit(model);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public void Transaction_Credit_Success_CreateNewTransaction()
        {
            // Arrange
            var user = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user.Id
            };

            var accounts = new List<Account>(){ account };

            var model = new TransactionalModel()
            {
                UserId = user.Id,
                AccountId = account.Id,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts.AsQueryable()).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Credit(model);

            // Assert
            Assert.True(result.IsSuccess);
            MockTransactionRepo.Verify(m => m.Add(It.IsAny<Transaction>()), Times.Once());
            MockUnitOfWork.Verify(m => m.Save(), Times.Once());
            Assert.True(account.Balance > 0);
        }

        [Fact]
        public void Transaction_Credit_Fail_UserAndIncorrectAccount()
        {
            // Arrange
            var user1 = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var user2 = new AppUser()
            {
                Id = 2,
                EmailAddress = "2@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account1 = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user1.Id
            };

            var account2 = new Account()
            {
                Id = 2,
                Name = "Test Account",
                Type = "Savings",
                UserId = user2.Id
            };

            var accounts = new List<Account>() { account1, account2 };

            var model = new TransactionalModel()
            {
                UserId = account1.UserId,
                AccountId = account2.Id,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts.AsQueryable()).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Credit(model);

            // Assert
            Assert.False(result.IsSuccess);
        }


        [Fact]
        public void Transaction_Credit_Success_CorrectUserAndAccount()
        {
            // Arrange
            var user1 = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var user2 = new AppUser()
            {
                Id = 2,
                EmailAddress = "2@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account1 = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user1.Id
            };

            var account2 = new Account()
            {
                Id = 2,
                Name = "Test Account",
                Type = "Savings",
                UserId = user2.Id
            };

            var accounts = new List<Account>() { account1, account2 };

            var model = new TransactionalModel()
            {
                UserId = account1.UserId,
                AccountId = account1.Id,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts.AsQueryable()).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Credit(model);

            // Assert
            Assert.True(result.IsSuccess);
            MockTransactionRepo.Verify(m => m.Add(It.IsAny<Transaction>()), Times.Once());
            MockUnitOfWork.Verify(m => m.Save(), Times.Once());
            Assert.True(account1.Balance > 0);
        }

        #endregion

        #region Debit Test

        [Fact]
        public void Transaction_Debit_Fail_UnknownIds()
        {
            // Arrange
            var user = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user.Id
            };

            var accounts = new List<Account>() { account }.AsQueryable();

            var model = new TransactionalModel()
            {
                UserId = 2,
                AccountId = 2,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Debit(model);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public void Transaction_Debit_Fail_UnknownAccount()
        {
            // Arrange
            var user = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user.Id
            };

            var accounts = new List<Account>() { account }.AsQueryable();

            var model = new TransactionalModel()
            {
                UserId = user.Id,
                AccountId = 2,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Debit(model);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public void Transaction_Debit_Fail_UnknownUser()
        {
            // Arrange
            var user = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user.Id
            };

            var accounts = new List<Account>() { account }.AsQueryable();

            var model = new TransactionalModel()
            {
                UserId = 2,
                AccountId = account.Id,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Debit(model);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public void Transaction_Debit_Success_CreateNewTransaction()
        {
            // Arrange
            var user = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user.Id
            };

            var initialBalance = 25000;
            account.AddBalance(initialBalance);

            var accounts = new List<Account>() { account };

            var model = new TransactionalModel()
            {
                UserId = user.Id,
                AccountId = account.Id,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts.AsQueryable()).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Debit(model);

            // Assert
            Assert.True(result.IsSuccess);
            MockTransactionRepo.Verify(m => m.Add(It.IsAny<Transaction>()), Times.Once());
            MockUnitOfWork.Verify(m => m.Save(), Times.Once());
            Assert.True(account.Balance < initialBalance);
        }

        [Fact]
        public void Transaction_Debit_Fail_UserAndIncorrectAccount()
        {
            // Arrange
            var user1 = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var user2 = new AppUser()
            {
                Id = 2,
                EmailAddress = "2@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account1 = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user1.Id
            };

            var account2 = new Account()
            {
                Id = 2,
                Name = "Test Account",
                Type = "Savings",
                UserId = user2.Id
            };

            var accounts = new List<Account>() { account1, account2 };

            var model = new TransactionalModel()
            {
                UserId = account1.UserId,
                AccountId = account2.Id,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts.AsQueryable()).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Debit(model);

            // Assert
            Assert.False(result.IsSuccess);
        }


        [Fact]
        public void Transaction_Debit_Success_CorrectUserAndAccount()
        {
            // Arrange
            var user1 = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var user2 = new AppUser()
            {
                Id = 2,
                EmailAddress = "2@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account1 = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user1.Id
            };

            var account2 = new Account()
            {
                Id = 2,
                Name = "Test Account",
                Type = "Savings",
                UserId = user2.Id
            };

            var initialBalance = 25000;
            account1.AddBalance(initialBalance);

            var accounts = new List<Account>() { account1, account2 };

            var model = new TransactionalModel()
            {
                UserId = account1.UserId,
                AccountId = account1.Id,
                Amount = 10000
            };

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts.AsQueryable()).Verifiable();

            // Act
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.Debit(model);

            // Assert
            Assert.True(result.IsSuccess);
            MockTransactionRepo.Verify(m => m.Add(It.IsAny<Transaction>()), Times.Once());
            MockUnitOfWork.Verify(m => m.Save(), Times.Once());
            Assert.True(account1.Balance < initialBalance);
        }

        #endregion

        #region GetAllPerAccount

        [Fact]
        public void Transaction_GetAll_Fail_UnknownIds()
        {
            // Arrange
            var user = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user.Id
            };

            var accounts = new List<Account>() { account }.AsQueryable();

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts).Verifiable();

            var transactions = new List<Transaction>() {
                new Transaction()
                    {
                        Id = 1,
                        AccountId = 2,
                        Amount = 10000,
                        TransactionType = TransactionType.Credit.ToString()
                    },
                new Transaction()
                    {
                        Id = 2,
                        AccountId = 2,
                        Amount = 10000,
                        TransactionType = TransactionType.Credit.ToString()
                    },
                new Transaction()
                    {
                        Id = 3,
                        AccountId = 2,
                        Amount = 10000,
                        TransactionType = TransactionType.Debit.ToString()
                    }
                }.AsQueryable();

            MockTransactionRepo.Setup(x => x.GetAll()).Returns(transactions).Verifiable();

            var model = new GetAllAccountTransactionsModel()
            {
                UserId = 2,
                AccountId = 2
            };

            // Act and Assert
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            //var result = transactionLogic.GetAllPerAccount(model);

            Assert.Throws<Exception>(() => transactionLogic.GetAllPerAccount(model));
        }

        [Fact]
        public void Transaction_GetAll_Fail_UnknownAccount()
        {
            // Arrange
            var user = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user.Id
            };

            var accounts = new List<Account>() { account }.AsQueryable();

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts).Verifiable();

            var transactions = new List<Transaction>() {
                new Transaction()
                    {
                        Id = 1,
                        AccountId = account.Id,
                        Amount = 10000,
                        TransactionType = TransactionType.Credit.ToString()
                    },
                new Transaction()
                    {
                        Id = 2,
                        AccountId = account.Id,
                        Amount = 10000,
                        TransactionType = TransactionType.Credit.ToString()
                    },
                new Transaction()
                    {
                        Id = 3,
                        AccountId = account.Id,
                        Amount = 10000,
                        TransactionType = TransactionType.Debit.ToString()
                    }
                }.AsQueryable();

            MockTransactionRepo.Setup(x => x.GetAll()).Returns(transactions).Verifiable();

            var model = new GetAllAccountTransactionsModel()
            {
                UserId = user.Id,
                AccountId = 2
            };

            // Act and Assert
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            //var result = transactionLogic.GetAllPerAccount(model);

            Assert.Throws<Exception>(() => transactionLogic.GetAllPerAccount(model));
        }

        [Fact]
        public void Transaction_GetAll_Success_GetUserTransaction()
        {
            // Arrange
            var user = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user.Id
            };

            var accounts = new List<Account>() { account }.AsQueryable();

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts).Verifiable();

            var transaction1 = new Transaction()
            {
                Id = 1,
                AccountId = account.Id,
                Amount = 10000,
                TransactionType = TransactionType.Credit.ToString(),
                TransactionDate = new DateTime(2000, 1, 1),
                DateCreated = new DateTime(2000, 1, 1)
            };

            var transaction2 = new Transaction()
            {
                Id = 1,
                AccountId = account.Id,
                Amount = 10000,
                TransactionType = TransactionType.Credit.ToString(),
                TransactionDate = new DateTime(2000, 1, 2),
                DateCreated = new DateTime(2000, 1, 2)
            };

            var transaction3 = new Transaction()
            {
                Id = 1,
                AccountId = account.Id,
                Amount = 10000,
                TransactionType = TransactionType.Credit.ToString(),
                TransactionDate = new DateTime(2000, 1, 3),
                DateCreated = new DateTime(2000, 1, 3)
            };

            var transactions = new List<Transaction>() {
                transaction1, transaction2, transaction3
                };

            MockTransactionRepo.Setup(x => x.GetAll()).Returns(transactions.AsQueryable()).Verifiable();

            var model = new GetAllAccountTransactionsModel()
            {
                UserId = account.UserId,
                AccountId = account.Id
            };

            // Act and Assert
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.GetAllPerAccount(model);

            foreach (var savedTransaction in result)
            {
                Assert.Contains(transactions, x =>
                    savedTransaction.Id == x.Id &&
                    savedTransaction.Amount == x.Amount &&
                    savedTransaction.TransactionDate == x.TransactionDate &&
                    savedTransaction.TransactionType == x.TransactionType
                );
            }
        }

        [Fact]
        public void Transaction_GetAll_Success_GetUserTransaction_MultipleOtherTransactions()
        {
            // Arrange
            var user1 = new AppUser()
            {
                Id = 1,
                EmailAddress = "1@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };
            var user2 = new AppUser()
            {
                Id = 2,
                EmailAddress = "2@Test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var account1 = new Account()
            {
                Id = 1,
                Name = "Test Account",
                Type = "Savings",
                UserId = user1.Id
            };

            var account2 = new Account()
            {
                Id = 2,
                Name = "Test Account",
                Type = "Savings",
                UserId = user2.Id
            };

            var accounts = new List<Account>() { account1, account2 }.AsQueryable();

            MockAccountRepo.Setup(x => x.GetAll()).Returns(accounts).Verifiable();

            var user1Transaction1 = new Transaction()
            {
                Id = 1,
                AccountId = account1.Id,
                Amount = 20000,
                TransactionType = TransactionType.Credit.ToString(),
                TransactionDate = new DateTime(2000, 1, 1),
                DateCreated = new DateTime(2000, 1, 1)
            };

            var user1Transaction2 = new Transaction()
            {
                Id = 2,
                AccountId = account1.Id,
                Amount = 10000,
                TransactionType = TransactionType.Credit.ToString(),
                TransactionDate = new DateTime(2000, 1, 2),
                DateCreated = new DateTime(2000, 1, 2)
            };

            var user1Transaction3 = new Transaction()
            {
                Id = 3,
                AccountId = account1.Id,
                Amount = 5000,
                TransactionType = TransactionType.Debit.ToString(),
                TransactionDate = new DateTime(2000, 1, 3),
                DateCreated = new DateTime(2000, 1, 3)
            };

            var user1Transactions = new List<Transaction>() {
                user1Transaction1, user1Transaction2, user1Transaction3
                };

            var user2Transaction1 = new Transaction()
            {
                Id = 4,
                AccountId = account2.Id,
                Amount = 30000,
                TransactionType = TransactionType.Credit.ToString(),
                TransactionDate = new DateTime(2000, 1, 1),
                DateCreated = new DateTime(2000, 1, 1)
            };

            var user2Transaction2 = new Transaction()
            {
                Id = 5,
                AccountId = account2.Id,
                Amount = 20000,
                TransactionType = TransactionType.Credit.ToString(),
                TransactionDate = new DateTime(2000, 1, 2),
                DateCreated = new DateTime(2000, 1, 2)
            };

            var user2Transaction3 = new Transaction()
            {
                Id = 6,
                AccountId = account2.Id,
                Amount = 10000,
                TransactionType = TransactionType.Debit.ToString(),
                TransactionDate = new DateTime(2000, 1, 3),
                DateCreated = new DateTime(2000, 1, 3)
            };

            var user2Transactions = new List<Transaction>() {
                user2Transaction1, user2Transaction2, user2Transaction3
                };

            var allTransactions = new List<Transaction>();

            allTransactions.AddRange(user1Transactions);
            allTransactions.AddRange(user2Transactions);

            MockTransactionRepo.Setup(x => x.GetAll()).Returns(allTransactions.AsQueryable()).Verifiable();

            var model = new GetAllAccountTransactionsModel()
            {
                UserId = account1.UserId,
                AccountId = account1.Id
            };

            // Act and Assert
            var transactionLogic = new TransactionLogic(MockUnitOfWork.Object,
                MockTransactionRepo.Object, MockAccountRepo.Object);

            var result = transactionLogic.GetAllPerAccount(model);

            foreach (var savedTransaction in result)
            {
                Assert.Contains(user1Transactions, x =>
                    savedTransaction.Id == x.Id &&
                    savedTransaction.Amount == x.Amount &&
                    savedTransaction.TransactionDate == x.TransactionDate &&
                    savedTransaction.TransactionType == x.TransactionType
);
            }
        }

        #endregion
    }
}
