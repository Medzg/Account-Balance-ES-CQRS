using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Events
{
    public class AccountBlocked : Event
    {
        public Guid AccountId { get;}

        public AccountBlocked(Guid accountId)
        {
            this.AccountId = accountId;
        }
    }
}
