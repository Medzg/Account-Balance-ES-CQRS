using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Commands
{
    public class CreateAccountCommand : ICommand
    {
        public Guid Id { get; } = Guid.NewGuid();

        public Guid AccountId { get; }
        public string AccountName { get; }

        public CreateAccountCommand(Guid accountId, string accountName)
        {
            this.AccountId = accountId;
            this.AccountName = accountName;
        }
    }
}
