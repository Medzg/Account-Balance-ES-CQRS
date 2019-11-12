using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Events
{
   public class ChequeDeposited : Event
    {
        public Guid AccountId { get;}

        public decimal Amount { get;}
        public DateTime TranscationDate { get;}

        public ChequeDeposited(Guid accountId,decimal amount,DateTime transcationDate)
        {
            this.AccountId = accountId;
            this.Amount = amount;
            this.TranscationDate = transcationDate;
        }
    }
}
