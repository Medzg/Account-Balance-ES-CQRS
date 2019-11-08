using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Events
{
    public class OverdraftLimitChanged : Event
    {
        public Guid AccountId { get; }
        public decimal Amount { get; }

        public OverdraftLimitChanged(Guid accountId , decimal amount)
        {
            this.AccountId = accountId;
            this.Amount = amount;
        }
    }
}
