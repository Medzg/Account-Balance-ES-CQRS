using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Commands
{
   public class SetDailyWireTransferLimitCommand : ICommand
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid AccountId { get; }
        public decimal Amount { get; }

        public SetDailyWireTransferLimitCommand(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
