using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Events
{
   public class CashDeposited : Event
    {
        public Guid AccountId { get; }

        public decimal Amount { get;}

        public CashDeposited(Guid accountId,decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
