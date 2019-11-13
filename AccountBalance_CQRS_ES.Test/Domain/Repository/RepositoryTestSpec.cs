﻿using AccountBalance_CQRS_ES.Domain.EStore;
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
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            Mock<IEventStore> mockEventSotre = new Mock<IEventStore>();
            List<IEvent> events = new List<IEvent>()
            {
                new AccountCreated(accountId,"Med"),
                new CashDeposited(accountId,200)

            };
            Myrepo.IRepository repo = new Myrepo.Repository(mockEventSotre.Object);
            mockEventSotre.Setup(x => x.GetByStreamId(It.IsAny<StreamIdentifier>())).Returns(Task.FromResult<IEnumerable<IEvent>>(events));

            var account = await repo.GetById<Account>(accountId);


            Assert.Equal("Med", account.AccountName);

        }

        [Fact]

        public async void id_not_excited_should_throw_exception()
        {
            Myrepo.IRepository repo = new Myrepo.Repository(Setup());
            var accountId = Guid.Parse("1405FB93-2A90-4C9C-B286-EE9A62A94212");



            

           await Assert.ThrowsAsync<InvalidOperationException>(()=> repo.GetById<Account>(accountId));

        }

     

  
        private IEventStore Setup()
        {
            return new EventStore();

        }
    }


}
