using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Events
{
     public class AccountUnblocked : Event
    {
        public Guid AccountId { get; }

        public AccountUnblocked(Guid accountId)
        {
            this.AccountId = accountId;
        }
    }
}
