using AccountBalance_CQRS_ES.Domain.Aggregate;
using AccountBalance_CQRS_ES.Domain.Commands;
using AccountBalance_CQRS_ES.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.CommandHandlers
{
    public class CommandHandler : 
        ICommandHandler<CreateAccountCommand>,
        ICommandHandler<SetDailyWireTransferLimitCommand>,
        ICommandHandler<DepositeCashCommand>,
        ICommandHandler<SetOverdraftLimitCommand>,
        ICommandHandler<WithdrawCashCommand>
    {
        private readonly IRepository _repo;
        public CommandHandler(IRepository repository)
        {
            _repo = repository;
        }
        private async Task Execute(Guid id, Action<Account> action)
        {
            var account = await _repo.GetById<Account>(id);
            action(account);
            await _repo.Save(account);
        }
        public async Task Handle(CreateAccountCommand cmd)
        {
            await _repo.Save(Account.Create(cmd.AccountId, cmd.AccountName));


        }

        public async Task Handle(SetDailyWireTransferLimitCommand cmd)
        {
            await Execute(cmd.AccountId, (account) => account.SetDailyWireTransferLimit(cmd.OverdraftLmit));
        }

        public async Task Handle(DepositeCashCommand cmd)
        {
            await Execute(cmd.AccountId, (account) => account.DepositeCash(cmd.Amount));
        }

        public async Task Handle(SetOverdraftLimitCommand cmd)
        {
            await Execute(cmd.AccountId, (accout) => accout.SetOverdraftLimit(cmd.Amount));
        }

        public async Task Handle(WithdrawCashCommand cmd)
        {
            await Execute(cmd.AccountId, (account) => account.WithdrawCash(cmd.Amount));
        }
    }
}
