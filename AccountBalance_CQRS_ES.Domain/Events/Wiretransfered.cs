using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Events
{
     public class WireTransfered : Event
    {

        public Guid AccountId { get;}

        public decimal Amount { get;}
        public WireTransfered(Guid accountId,decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
