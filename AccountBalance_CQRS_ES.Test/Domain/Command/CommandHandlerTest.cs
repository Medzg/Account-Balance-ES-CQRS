using AccountBalance_CQRS_ES.Domain.CommandHandlers;
using AccountBalance_CQRS_ES.Domain.EStore;
using Myrepo = AccountBalance_CQRS_ES.Domain.Repository;
using System;
using System.Threading.Tasks;
using AccountBalance_CQRS_ES.Domain.Commands;
using AccountBalance_CQRS_ES.Domain.Aggregate;
using Xunit;

namespace AccountBalance_CQRS_ES.Test.Domain.Command
{
     public class CommandHandlerTest
    {
        [Fact]
        public async Task create_account_command_should_create_an_accountAsync()
        {
            var commandHandler = setup();
            var accountId = Guid.Parse("1305FB93-2A90-4C9C-B286-EE9A62A94212");
            CreateAccountCommand createAccount = new CreateAccountCommand(accountId, "Mohamed Zghal");
            await commandHandler.Handle(createAccount);

           var account = await GetAccount(accountId);
            Assert.Equal("Mohamed Zghal", account.AccountName);
           
        }
        [Fact]
        public async Task add_debt_to_account_should_update_account_debt_amount()
        {

            var commandHandler = setup();
            var accountId = Guid.Parse("1305FB93-2A90-4C9C-B286-EE9A62A94212");
            var CurrentState = await GetAccount(accountId);
            var currentDebt = CurrentState.Debt;
            DepositeCashCommand depositeCash = new DepositeCashCommand(accountId, 200);

            await commandHandler.Handle(depositeCash);
            CurrentState = await GetAccount(accountId);
            Assert.Equal(currentDebt + 200, CurrentState.Debt);
        }

       
        [Theory]
        [InlineData(0)]
        [InlineData(-10)]

        public async Task add_negative_amount_debt_to_account_should_update_account_debt_amount(decimal amount)
        {
            var commandHandler = setup();
            var accountId = Guid.Parse("1305FB93-2A90-4C9C-B286-EE9A62A94212");
            DepositeCashCommand depositeCash = new DepositeCashCommand(accountId, amount);

            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => commandHandler.Handle(depositeCash));
        }
        [Fact]
        public async Task WithdrawCash_command_need_update_debt_ammountAsync()
        {

            var commandhandler = setup();
            var accountId = Guid.Parse("1305FB93-2A90-4C9C-B286-EE9A62A94212");
            var account = await GetAccount(accountId);
            var currentDebt = account.Debt;
            WithdrawCashCommand withdrawCash = new WithdrawCashCommand(accountId, 200);
            await commandhandler.Handle(withdrawCash);
            account = await GetAccount(accountId);
            Assert.Equal(currentDebt-200,account.Debt);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task WithdrawCash_command_thorw_exception_when_amount_is_negative_or0(decimal amount)
        {

            var commandhandler = setup();
            var accountId = Guid.Parse("1305FB93-2A90-4C9C-B286-EE9A62A94212");
            var account = await GetAccount(accountId);
            WithdrawCashCommand withdrawCash = new WithdrawCashCommand(accountId, amount);
            await Assert.ThrowsAsync<InvalidOperationException>(() => commandhandler.Handle(withdrawCash));

        }


        [Fact]
        public async Task set_wire_transfer_limit_command_should_thorw_excption_if_set_negativeAsync()
        {
            var commandhandler = setup();
            var accountId = Guid.Parse("1305FB93-2A90-4C9C-B286-EE9A62A94212");
            SetDailyWireTransferLimitCommand setDailyWireTransferLimit = new SetDailyWireTransferLimitCommand(accountId, -100);

            await Assert.ThrowsAsync<InvalidOperationException>(() => commandhandler.Handle(setDailyWireTransferLimit));
        }
        [Fact]
        public async Task set_overdraft_limit_command_should_thorw_excption_if_set_negativeAsync()
        {
            var commandhandler = setup();
            var accountId = Guid.Parse("1305FB93-2A90-4C9C-B286-EE9A62A94212");
            SetOverdraftLimitCommand SetOverdraftLimit = new SetOverdraftLimitCommand(accountId, -100);

            await Assert.ThrowsAsync<InvalidOperationException>(() => commandhandler.Handle(SetOverdraftLimit));
        }

        [Fact]
        public async Task wire_transfer_limit_command_should_update_debt_amount()
        {
            var commandhandler = setup();
            var accountId = Guid.Parse("1305FB93-2A90-4C9C-B286-EE9A62A94212");
            var account = await GetAccount(accountId);
            var currentDebt = account.Debt;
            SetDailyWireTransferLimitCommand setDailyWireTransferLimit = new SetDailyWireTransferLimitCommand(accountId, 1500);

            WireTransferCommand wireTransfer = new WireTransferCommand(accountId, 100);

            await commandhandler.Handle(setDailyWireTransferLimit);

            await commandhandler.Handle(wireTransfer);
            account = await GetAccount(accountId);
            Assert.Equal(currentDebt - 100, account.Debt);

        }
        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public async Task wire_transfert_command_should_throw_excpetion_if_amount_negative_or0(decimal amount)
        {
            var commandhandler = setup();
            var accountId = Guid.Parse("1305FB93-2A90-4C9C-B286-EE9A62A94212");
            var account = await GetAccount(accountId);
          
            WireTransferCommand wireTransfer = new WireTransferCommand(accountId, amount);

          await  Assert.ThrowsAsync<InvalidOperationException>(() => commandhandler.Handle(wireTransfer));
        }


        private CommandHandler setup()
        {

            IEventStore es = new EventStore();
            Myrepo.IRepository repo = new Myrepo.Repository(es);
            return new CommandHandler(repo); 
        }

        private async Task<Account> GetAccount(Guid accountId)
        {

            IEventStore es = new EventStore();
            Myrepo.IRepository repo = new Myrepo.Repository(es);
            return await repo.GetById<Account>(accountId);
        }
    }
}
