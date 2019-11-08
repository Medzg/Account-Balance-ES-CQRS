using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Events
{
    public class AccountCreated : Event
    {
        public Guid AccountId { get; }
        public string AccountName { get; }

        
        public AccountCreated(Guid accountId, string accountname)
        {
            AccountId = accountId;
            AccountName = accountname;
        }
    }
}
