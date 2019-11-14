using AccountBalance_CQRS_ES.Domain.EStore;
using Myrepo = AccountBalance_CQRS_ES.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using AccountBalance_CQRS_ES.Domain.Aggregate;
using AccountBalance_CQRS_ES.Domain.Helpers;
using AccountBalance_CQRS_ES.Domain.Events;

namespace AccountBalance_CQRS_ES.Test.Domain.Repository
{
   public class RepositoryTestSpec
    {
        

        [Fact]

        public async void getting_an_item_by_id()
        {
            #region Arrange
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            Mock<IEventStore> mockEventSotre = new Mock<IEventStore>();
            Myrepo.IRepository repo = new Myrepo.Repository(mockEventSotre.Object);
            mockEventSotre.Setup(x => x.GetByStreamId(It.IsAny<StreamIdentifier>())).Returns(Task.FromResult<IEnumerable<IEvent>>(GetEvents()));
            #endregion
            #region Act
            var account = await repo.GetById<Account>(accountId);
            #endregion
            #region Assert
            Assert.Equal("Med", account.AccountName);
            Assert.Equal(200, account.Debt);
            #endregion
        }



        [Fact]

        public async Task id_not_excited_should_throw_exception()
        {
            #region Arrange
            Mock<IEventStore> mockEventSotre = new Mock<IEventStore>();
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            mockEventSotre.Setup(x => x.GetByStreamId(It.IsAny<StreamIdentifier>())).Throws<InvalidOperationException>();
            Myrepo.IRepository repo = new Myrepo.Repository(mockEventSotre.Object);
            #endregion
            #region Act and Assert cause async callback
            await Assert.ThrowsAsync<InvalidOperationException>(()=> repo.GetById<Account>(accountId));
            #endregion
        }



        [Fact]
        public async Task Save_should_be_called_once()
        {
            #region Arrange
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            Mock<IEventStore> mockEventSotre = new Mock<IEventStore>();
            mockEventSotre.Setup(x => x.Save(It.IsAny<List<EventStoreStream>>()));

            Myrepo.IRepository repo = new Myrepo.Repository(mockEventSotre.Object);
            var account = Account.Create(accountId, "Med");
            #endregion
            #region Act
            await repo.Save(account);
            #endregion
            mockEventSotre.Verify(x => x.Save(It.IsAny<List<EventStoreStream>>()), Times.Once);
   
        }

        private List<IEvent> GetEvents()
        {
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            return new List<IEvent>()
            {
                new AccountCreated(accountId,"Med"),
                new CashDeposited(accountId,200)

            };
        }

    }


}
