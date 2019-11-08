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
        public decimal OverdraftLmit { get; }

        public SetDailyWireTransferLimitCommand(Guid accountId, decimal overdraftLimit)
        {
            AccountId = accountId;
            OverdraftLmit = overdraftLimit;
        }
    }
}
