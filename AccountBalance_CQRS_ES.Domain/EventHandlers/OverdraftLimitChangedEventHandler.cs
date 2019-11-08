using AccountBalance_CQRS_ES.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.EventHandlers
{
    public class OverdraftLimitChangedEventHandler : IEventHandler<OverdraftLimitChanged>
    {
        public string Id { get; }

        public List<OverdraftLimitChanged> Events { get; set; }
    }
}
