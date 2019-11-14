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
            #region Arrange
            var accountId = Guid.NewGuid();
            #endregion
            #region Act
            var account = Account.Create(accountId, "Med");
            #endregion
            #region Assert
            Assert.NotNull(account);
            Assert.Equal("Med", account.AccountName);
            #endregion

        }
        [Fact]
        public void account_should_throw_excption_if_there_no_name_or_account_id()
        {
            #region Arrange
            var accountId = Guid.Empty;
            #endregion
            #region Act
            Action action = () => { var account = Account.Create(accountId, ""); };
            #endregion
            #region Assert
            Assert.Throws<InvalidOperationException>(action);
            #endregion
        }


        [Fact]
        public void account_should_add_debt_amount()
        {
            #region Arrange
            var accountId = Guid.NewGuid();
            var account = Account.Create(accountId, "Med");
            #endregion
            #region Act
            account.DepositeCash(100);
            #endregion
            #region Assert
            Assert.Equal(100, account.Debt);
            #endregion
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void account_should_throw_exception_when_add_debt_negative_amount(decimal amount)
        {
            #region Arrange
            var accountId = Guid.NewGuid();
            var account = Account.Create(accountId, "Med");
            #endregion
            #region Act
            Action action  = ()=> account.DepositeCash(amount);
            #endregion
            #region Assert
            Assert.Throws<InvalidOperationException>(action);
            #endregion
        }

        [Fact]
        public void WithdrawCash_need_update_debt_ammount()
        {
            #region Arrange
            var account = SetupAccount();
            account.DepositeCash(100);
            #endregion
            #region Act
            account.WithdrawCash(50);
            #endregion
            #region Assert
            Assert.Equal(50, account.Debt);
            #endregion
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void withdraw_negative_amount_should_throw_exception(decimal amount)
        {
            #region Arrange
            var account = SetupAccount();
            account.DepositeCash(100);
            #endregion
            #region Act
            Action action = ()=>  account.WithdrawCash(amount);
            #endregion
            #region Assert
            Assert.Throws<InvalidOperationException>(action);
            Assert.Equal(100, account.Debt);
            #endregion
        }
        [Fact]
        public void wire_transfer_limit_should_thorw_excption_if_set_negative()
        {
            #region Arrange
            var account = SetupAccount();
            #endregion
            #region Act
            Action action = () => { account.SetDailyWireTransferLimit(-100); };
            #endregion
            #region Assert
            Assert.Throws<InvalidOperationException>(action);
            #endregion
        }

        [Fact]
        public void daily_limit_should_thorw_excption_if_set_negative()
        {
            #region Arrange
            var account = SetupAccount();
            #endregion
            #region Act
            Action action = () => { account.SetOverdraftLimit(-100); };
            #endregion
            #region Assert
            Assert.Throws<InvalidOperationException>(action);
            #endregion
        }


        [Fact]

        public void wire_transfer_should_update_debt()
        {
            #region Arrange
            var account = SetupAccount();
            account.DepositeCash(100);
            account.SetDailyWireTransferLimit(500);
            #endregion
            #region Act
            account.WireTransfert(50);
            #endregion
            #region Assert
            Assert.Equal(50, account.Debt);
            #endregion
        }


        [Fact]
        public void wire_transfert_should_throw_excption_if_pass_the_limit_and_set_account_to_blocked()
        {
            #region Arrange
            var account = SetupAccount();
            account.DepositeCash(100);
            account.SetDailyWireTransferLimit(10);
            #endregion
            #region Act
            Action action = () => account.WireTransfert(50);
            #endregion
            #region Assert
            Assert.Throws<InvalidOperationException>(action);
            Assert.Equal(State.blocked, account.AccountState);
            #endregion

        }

        [Fact]
        public void withdraw_should_throw_excption_if_pass_the_limit_and_set_account_to_blocked()
        {
            #region Arrange
            var account = SetupAccount();
            account.DepositeCash(100);
            account.SetOverdraftLimit(10);
            #endregion
            #region Act
            Action action = () => account.WithdrawCash(500);
            #endregion
            #region Assert
            Assert.Throws<InvalidOperationException>(action);
            Assert.Equal(State.blocked, account.AccountState);
            #endregion

        }


        [Fact]
        public void withdraw_should_throw_excption_if_account_already_blo()
        {
            #region Arrange
            var account = SetupAccount();
            account.DepositeCash(100);
            account.SetOverdraftLimit(10);
            #endregion

            #region Act and assert to test behavior num 1
            Action action = () => account.WithdrawCash(500);
            Assert.Throws<InvalidOperationException>(action);
            #endregion

            #region Act and assert to test behavior num 2
            Action action1 = () => account.WithdrawCash(20);
            Assert.Throws<InvalidOperationException>(action1);
            Assert.Equal(State.blocked, account.AccountState);
            #endregion
        }

        private Account SetupAccount()
        {

            var accountId = Guid.NewGuid();
            return Account.Create(accountId, "Med");
        }
    }
}
