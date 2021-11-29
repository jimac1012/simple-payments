using Application;
using Domain;
using Moq;
using System.Linq;
using Repository.Interfaces;
using System.Collections.Generic;
using Xunit;
using Model;

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



        #endregion
    }
}
