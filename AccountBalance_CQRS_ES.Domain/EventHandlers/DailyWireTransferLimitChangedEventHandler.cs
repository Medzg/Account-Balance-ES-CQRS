using AccountBalance_CQRS_ES.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.EventHandlers
{
    public class DailyWireTransferLimitChangedEventHandler : IEventHandler<DailyWireTransferLimitChanged>
    {
        public string Id { get; set; }
        public List<DailyWireTransferLimitChanged> Events { get; set; }
    }
}
