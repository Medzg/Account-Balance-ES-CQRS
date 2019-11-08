using AccountBalance_CQRS_ES.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Helpers
{
    public class EventStoreStream
    {
        public EventStoreStream(StreamIdentifier identifier, IEnumerable<IEvent> events)
        {
            this.Id = identifier.Value;
            this.Events = events.ToList();
        }
        public List<IEvent> Events { get; set; }
        public string Id { get; set; }
    }
}
