using AccountBalance_CQRS_ES.Domain.Events;
using AccountBalance_CQRS_ES.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.EStore
{
   public interface IEventStore
    {
         
        Task<IEnumerable<IEvent>> GetByStreamId(StreamIdentifier streamId);

        Task Save(List<EventStoreStream> newEvents);
    }
}
