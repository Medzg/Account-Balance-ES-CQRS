using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountBalance_CQRS_ES.Domain.Aggregate;
using AccountBalance_CQRS_ES.Domain.EStore;
using AccountBalance_CQRS_ES.Domain.Helpers;

namespace AccountBalance_CQRS_ES.Domain.Repository
{
    public class Repository : IRepository
    {
        private readonly IEventStore _eventStore;
        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

       

        public async Task<T> GetById<T>(Guid id) where T : AggregateRoot, new()
        {
            var streamItem = new T();
            var streamId = new StreamIdentifier(streamItem.GetType().Name, id);
            var history = await _eventStore.GetByStreamId(streamId);
            streamItem.LoadFromHistory(history);
            return streamItem;


        }

        public async Task Save(params AggregateRoot[] streamItems)
        {
            var newEvents = new List<EventStoreStream>();
            foreach (var item in streamItems)
            {
                newEvents.Add(new EventStoreStream(item.StreamIdentifier, item.GetUncommittedChanges()));
            }
            await _eventStore.Save(newEvents);
            foreach (var item in streamItems)
            {
                item.Commit();
            }
        }
    }
}
