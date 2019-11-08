using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Commands
{
    public class WithdrawCashCommand : ICommand
    {
        public Guid Id { get; } = Guid.NewGuid();

        public Guid AccountId { get;}

        public decimal Amount { get;}

        public WithdrawCashCommand(Guid accountId,decimal amount)
        {
            this.AccountId = accountId;
            this.Amount = amount;
        }
    }
}
