using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Events
{
    public class DailyWireTransferLimitChanged : Event
    {
        public Guid AccountId { get; set; }

        public decimal DailyWireTransferLimit { get; set; }

        public DailyWireTransferLimitChanged(Guid accountId, decimal dailyWireTransferLimit)
        {
            AccountId = accountId;
            DailyWireTransferLimit = dailyWireTransferLimit;
        }
    }
}
