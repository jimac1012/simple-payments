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
        private Mock<IUnitOfWork> MockUnitOfWork { get; }
        private Mock<IGenericRepository<Transaction>> MockTransactionRepo { get; }
        private Mock<IGenericRepository<Account>> MockAccountRepo { get; }

        public TransactionLogicTest()
        {
            MockUnitOfWork = new Mock<IUnitOfWork>();
            MockTransactionRepo = new Mock<IGenericRepository<Transaction>>();
            MockAccountRepo = new Mock<IGenericRepository<Account>>();
        }

        [Fact]
        public void Credit_Account()
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

            transactionLogic.Credit(model);

            // Assert
            var transaction = Mock.Get(MockTransactionRepo.Object.GetAll().FirstOrDefault()).Object;

            Assert.NotNull(transaction);
            Assert.Equal(model.AccountId, transaction.AccountId);
            Assert.Equal(model.Amount, transaction.Amount);
        }
    }
}
