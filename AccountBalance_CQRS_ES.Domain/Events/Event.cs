using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Events
{
    public class Event: IEvent
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
    
    }
}
