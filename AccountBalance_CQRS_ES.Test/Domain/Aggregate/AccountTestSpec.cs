using AccountBalance_CQRS_ES.Domain.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AccountBalance_CQRS_ES.Test.Domain.Aggregate
{
 
   public class AccountTestSpec
    {
        
        [Fact]
        public void Account_should_created_with_event()
        {
            var accountId = Guid.NewGuid();
            var account = Account.Create(accountId, "Med");

            Assert.NotNull(account);
            Assert.Equal("Med", account.AccountName);


        }
        [Fact]
        public void account_should_throw_excption_if_there_no_name_or_account_id()
        {
            var accountId = Guid.Empty;
            Action action = () => { var account = Account.Create(accountId, ""); };
            Assert.Throws<InvalidOperationException>(action);
        }


        [Fact]
        public void account_should_add_debt_amount()
        {

            var accountId = Guid.NewGuid();
            var account = Account.Create(accountId, "Med");
            account.DepositeCash(100);
            Assert.Equal(100, account.Debt);
        }
      

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void account_should_throw_exception_when_add_debt_negative_amount(decimal amount)
        {
            var accountId = Guid.NewGuid();
            var account = Account.Create(accountId, "Med");
            Action  action  = ()=> account.DepositeCash(amount);
            Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void WithdrawCash_need_update_debt_ammount()
        {

            var account = SetupAccount();
            account.DepositeCash(100);
            account.WithdrawCash(50);

            Assert.Equal(50, account.Debt);
            
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void withdraw_negative_amount_should_throw_exception(decimal amount)
        {
            var account = SetupAccount();
            account.DepositeCash(100);


            Action action = ()=>  account.WithdrawCash(amount);

            Assert.Throws<InvalidOperationException>(action);
            Assert.Equal(100, account.Debt);

        }
        [Fact]
        public void wire_transfer_limit_should_thorw_excption_if_set_negative()
        {
            var account = SetupAccount();
            Action action = () => { account.SetDailyWireTransferLimit(-100); };
            Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void daily_limit_should_thorw_excption_if_set_negative()
        {
            var account = SetupAccount();
            Action action = () => { account.SetOverdraftLimit(-100); };
            Assert.Throws<InvalidOperationException>(action);
        }


        [Fact]

        public void wire_transfer_should_update_debt()
        {
            var account = SetupAccount();
            account.DepositeCash(100);
            account.SetDailyWireTransferLimit(500);

            account.WireTransfert(50);

            Assert.Equal(50, account.Debt);
        }


        [Fact]
        public void wire_transfert_should_throw_excption_if_pass_the_limit_and_set_account_to_blocked()
        {
            var account = SetupAccount();
            account.DepositeCash(100);
            account.SetDailyWireTransferLimit(10);

            Action action = () => account.WireTransfert(50);

            Assert.Throws<InvalidOperationException>(action);
            Assert.Equal(State.blocked, account.AccountState);
        }

        [Fact]
        public void withdraw_should_throw_excption_if_pass_the_limit_and_set_account_to_blocked()
        {
            var account = SetupAccount();
            account.DepositeCash(100);
            account.SetOverdraftLimit(10);

            Action action = () => account.WithdrawCash(500);

            Assert.Throws<InvalidOperationException>(action);
            Assert.Equal(State.blocked, account.AccountState);
        }


        [Fact]
        public void withdraw_should_throw_excption_if_account_already_blo()
        {
            var account = SetupAccount();
            account.DepositeCash(100);
            account.SetOverdraftLimit(10);

            Action action = () => account.WithdrawCash(500);

            Assert.Throws<InvalidOperationException>(action);
            Action action1 = () => account.WithdrawCash(20);
            Assert.Throws<InvalidOperationException>(action1);
            Assert.Equal(State.blocked, account.AccountState);
        }

        private Account SetupAccount()
        {

            var accountId = Guid.NewGuid();
            return Account.Create(accountId, "Med");
        }
    }
}
