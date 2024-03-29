﻿using AccountBalance_CQRS_ES.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Aggregate
{
    public enum State { unblocked , blocked}
    public class Account : AggregateRoot
    {
       

        private decimal _overdraftLimit;

        private decimal _dailyWireTransferLimit;

        public decimal WithDrawnToday { get; private set;}

        public State AccountState{ get; private set; }
      
        public string AccountName { get; private set; }
        public decimal Debt { get; private set; }
      
        public Account() { }
       
        private Account(Guid accountId, string accountName)
        {
            if (accountId == null || Guid.Empty == accountId)
                throw new InvalidOperationException("You can't create account without account Id");
            if (string.IsNullOrEmpty(accountName))
                throw new InvalidOperationException("you can't create account without account name");
            this.ApplyChanges(new AccountCreated(accountId, accountName));
        }
        private void Apply(AccountCreated @event)
        {
            this.Id = @event.AccountId;
            this.AccountName = @event.AccountName;
        }
        private void Apply(DailyWireTransferLimitChanged @event)
        {
          
            _dailyWireTransferLimit = @event.DailyWireTransferLimit;
        }

        private void Apply(CashDeposited @event)
        {
            
            this.Debt += @event.Amount;
        }

        private void Apply(OverdraftLimitChanged @event)
        {
            _overdraftLimit = @event.Amount;
        }

        private void Apply(AccountBlocked @event)
        {
            AccountState = State.blocked;
        }

        private void Apply(AccountUnblocked @event)
        {
            AccountState = State.unblocked;
        }

        private void Apply(CashWithdrawn @event)
        {
            this.Debt -= @event.Amount;
        }
        private void Apply(WireTransfered @event)
        {
            this.Debt -= @event.Amount;
            this.WithDrawnToday += @event.Amount;
        }
        private void Apply(ChequeDeposited @event)
        {
            TimeSpan ts = new TimeSpan(09, 00, 0);
            if (@event.TranscationDate >= DateTime.UtcNow.Date + ts)
                Debt += @event.Amount;
        }

        protected  override void RegisterAppliers()
        {
            this.RegisterAppliers<AccountCreated>(this.Apply);
            this.RegisterAppliers<DailyWireTransferLimitChanged>(this.Apply);
            this.RegisterAppliers<CashDeposited>(this.Apply);
            this.RegisterAppliers<OverdraftLimitChanged>(this.Apply);
            this.RegisterAppliers<AccountBlocked>(this.Apply);
            this.RegisterAppliers<CashWithdrawn>(this.Apply);
            this.RegisterAppliers<AccountUnblocked>(this.Apply);
            this.RegisterAppliers<WireTransfered>(this.Apply);
            this.RegisterAppliers<ChequeDeposited>(this.Apply);
        }

      
        public static Account Create(Guid AccountId, string accountName)
        {
            return new Account(AccountId, accountName);
        }

        public void SetDailyWireTransferLimit(decimal amount)
        {

            if (amount < 0)
                throw new InvalidOperationException("Daily wire transfer limit can't be negative");
            this.ApplyChanges(new DailyWireTransferLimitChanged(this.Id, amount));

        }
        public void SetOverdraftLimit(decimal overdraftLimit)
        {
              if (overdraftLimit < 0)
                throw new InvalidOperationException("overdraft limit can't be negative");
            this.ApplyChanges(new OverdraftLimitChanged(this.Id, overdraftLimit));

        }
        
        public void DepositeCash(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("can't deposite negative amount of cash");

            this.ApplyChanges(new CashDeposited(this.Id, amount));

            if (AccountState == State.blocked && (Debt + amount) >= 0)
            {
                this.ApplyChanges(new AccountUnblocked(this.Id));
            }
        }

        public void WithdrawCash(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Can't withdraw a negative amount of cash");
            if (AccountState == State.blocked)
                throw new InvalidOperationException("Can't withdraw, your account is temporarily locked ");
            if((Debt - amount) < 0 &&  Math.Abs(Debt-amount) > _overdraftLimit)
            {
                this.ApplyChanges(new AccountBlocked(this.Id));
                throw new InvalidOperationException("you pass your overdraftLimit, now your account is locked ");
            }
            else
            {

                this.ApplyChanges(new CashWithdrawn(this.Id, amount));
            }

        }
      
       public void WireTransfert(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Can't transfer a negative amount");
            if (AccountState == State.blocked)
                throw new InvalidOperationException("Can't transfer an amount while the account is blocked");
            if (_dailyWireTransferLimit <= WithDrawnToday + amount) { 
                this.ApplyChanges(new AccountBlocked(this.Id));
                throw new InvalidOperationException(string.Format("You can't pass your daily wire transfert limit : {0} . Now your account is blocked", _dailyWireTransferLimit));
                
            }
            else
                this.ApplyChanges(new WireTransfered(this.Id, amount));
            




        }
        public void DepositCheque(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("can't deposite a negative amount of cheque");
            var date = DateTime.Now;
            var transactionDate = Helpers.Helper.GetNextBusinessDay(date);
            this.ApplyChanges(new ChequeDeposited(this.Id, amount, transactionDate));
        }
    }
}
