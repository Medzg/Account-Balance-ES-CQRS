﻿using AccountBalance_CQRS_ES.Domain.CommandHandlers;
using AccountBalance_CQRS_ES.Domain.EStore;
using Myrepo = AccountBalance_CQRS_ES.Domain.Repository;
using System;
using System.Threading.Tasks;
using AccountBalance_CQRS_ES.Domain.Commands;
using AccountBalance_CQRS_ES.Domain.Aggregate;
using Xunit;
using Moq;
using System.Collections.Generic;
using AccountBalance_CQRS_ES.Domain.Events;

namespace AccountBalance_CQRS_ES.Test.Domain.Command
{
     public class CommandHandlerTest
    {
              
        
        [Fact]
        public async Task add_debt_to_account_should_update_account_debt_amount()
        {
            #region Arrange
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            Account account = GetAccount();
            // setup the object that's need to return in every call of get object
            var repo = new Mock<Myrepo.IRepository>();
            repo.Setup(x => x.GetById<Account>(It.IsAny<Guid>())).Returns(Task.FromResult(account));
            CommandHandler commandHandler = new CommandHandler(repo.Object);
            #endregion
            #region Act
            await commandHandler.Handle(new DepositeCashCommand(accountId, 200));
            #endregion

            #region Assert
            Assert.Equal(200, account.Debt);
            #endregion

        }


        [Theory]
        [InlineData(0)]
        [InlineData(-10)]

        public async Task add_negative_amount_debt_to_account_should_update_account_debt_amount(decimal amount)
        {
            #region Arrange

            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            Account account = GetAccount();
            var repo = new Mock<Myrepo.IRepository>();
            repo.Setup(x => x.GetById<Account>(It.IsAny<Guid>())).Returns(Task.FromResult(account));
            CommandHandler commandHandler = new CommandHandler(repo.Object);
            DepositeCashCommand depositeCash = new DepositeCashCommand(accountId, amount);
            #endregion

            #region Act and Assert cause call async
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => commandHandler.Handle(depositeCash));
            #endregion
        }
        [Fact]
        public async Task WithdrawCash_command_need_update_debt_ammountAsync()
        {
            #region Arrange
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            Account account = GetAccount();
            var repo = new Mock<Myrepo.IRepository>();
            repo.Setup(x => x.GetById<Account>(It.IsAny<Guid>())).Returns(Task.FromResult(account));
            CommandHandler commandHandler = new CommandHandler(repo.Object);
            await commandHandler.Handle(new DepositeCashCommand(accountId, 200));

            #endregion


            #region Act
            WithdrawCashCommand withdrawCash = new WithdrawCashCommand(accountId, 200);

            await commandHandler.Handle(withdrawCash);
            #endregion


            #region Assert
            Assert.Equal(0,account.Debt);
            #endregion
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task WithdrawCash_command_thorw_exception_when_amount_is_negative_or0(decimal amount)
        {

            #region Arrange
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            Account account = GetAccount();
            var repo = new Mock<Myrepo.IRepository>();
            repo.Setup(x => x.GetById<Account>(It.IsAny<Guid>())).Returns(Task.FromResult(account));
            CommandHandler commandHandler = new CommandHandler(repo.Object);
            await commandHandler.Handle(new DepositeCashCommand(accountId, 200));
            WithdrawCashCommand withdrawCash = new WithdrawCashCommand(accountId, amount);
            #endregion
      
            #region Act and Assert cause call async

            await Assert.ThrowsAsync<InvalidOperationException>(() => commandHandler.Handle(withdrawCash));
            #endregion
        }


        [Fact]
        public async Task set_wire_transfer_limit_command_should_thorw_excption_if_set_negativeAsync()
        {
            #region Arrange
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            Account account = GetAccount();
            var repo = new Mock<Myrepo.IRepository>();
            repo.Setup(x => x.GetById<Account>(It.IsAny<Guid>())).Returns(Task.FromResult(account));
            CommandHandler commandHandler = new CommandHandler(repo.Object);

            SetDailyWireTransferLimitCommand setDailyWireTransferLimit = new SetDailyWireTransferLimitCommand(accountId, -100);
            #endregion

            #region Act and Assert cause call async
            await Assert.ThrowsAsync<InvalidOperationException>(() => commandHandler.Handle(setDailyWireTransferLimit));
            #endregion
        }
        [Fact]
        public async Task set_overdraft_limit_command_should_thorw_excption_if_set_negativeAsync()
        {
            #region Arrange
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            Account account = GetAccount();
            var repo = new Mock<Myrepo.IRepository>();
            repo.Setup(x => x.GetById<Account>(It.IsAny<Guid>())).Returns(Task.FromResult(account));
            CommandHandler commandHandler = new CommandHandler(repo.Object);
            #endregion
            SetOverdraftLimitCommand SetOverdraftLimit = new SetOverdraftLimitCommand(accountId, -100);

            await Assert.ThrowsAsync<InvalidOperationException>(() => commandHandler.Handle(SetOverdraftLimit));
        }

        [Fact]
        public async Task wire_transfer_limit_command_should_update_debt_amount()
        {
            #region Arrange
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            Account account = GetAccount();
            var repo = new Mock<Myrepo.IRepository>();
            repo.Setup(x => x.GetById<Account>(It.IsAny<Guid>())).Returns(Task.FromResult(account));
            CommandHandler commandHandler = new CommandHandler(repo.Object);
            SetDailyWireTransferLimitCommand setDailyWireTransferLimit = new SetDailyWireTransferLimitCommand(accountId, 1500);
            WireTransferCommand wireTransfer = new WireTransferCommand(accountId, 100);
            #endregion

            await commandHandler.Handle(new DepositeCashCommand(accountId, 200));
            await commandHandler.Handle(setDailyWireTransferLimit);

            await commandHandler.Handle(wireTransfer);
          
            Assert.Equal( 100, account.Debt);

        }
        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public async Task wire_transfert_command_should_throw_excpetion_if_amount_negative_or0(decimal amount)
        {
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            Account account = GetAccount();
            var repo = new Mock<Myrepo.IRepository>();
            repo.Setup(x => x.GetById<Account>(It.IsAny<Guid>())).Returns(Task.FromResult(account));
            CommandHandler commandHandler = new CommandHandler(repo.Object);

            WireTransferCommand wireTransfer = new WireTransferCommand(accountId, amount);

          await  Assert.ThrowsAsync<InvalidOperationException>(() => commandHandler.Handle(wireTransfer));
        }


        

        private Account GetAccount()
        {
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            return Account.Create(accountId, "Med");
        }

    
    }
}
