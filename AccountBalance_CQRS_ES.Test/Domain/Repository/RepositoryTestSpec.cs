using AccountBalance_CQRS_ES.Domain.EStore;
using Myrepo = AccountBalance_CQRS_ES.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AccountBalance_CQRS_ES.Domain.Aggregate;

namespace AccountBalance_CQRS_ES.Test.Domain.Repository
{
   public class RepositoryTestSpec
    {

        [Fact]

        public async void getting_an_item_by_id()
        {
            Myrepo.IRepository repo = new Myrepo.Repository(Setup());
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");

            var account = await repo.GetById<Account>(accountId);

            Assert.NotNull(account);

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
