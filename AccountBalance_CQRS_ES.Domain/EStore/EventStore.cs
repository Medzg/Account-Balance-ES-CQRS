using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountBalance_CQRS_ES.Domain.Events;
using AccountBalance_CQRS_ES.Domain.Helpers;
using EventStore.ClientAPI;

namespace AccountBalance_CQRS_ES.Domain.EStore
{
    public class EventStore : IEventStore
    {

        private IEventStoreConnection _connection;

        public EventStore()
        {
            try { 

            connect();
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        private async void connect()
        {
            
            _connection = EventStoreConnection.Create(System.Configuration.ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString);
            await _connection.ConnectAsync();
        }

        public async Task<IEnumerable<IEvent>> GetByStreamId(StreamIdentifier streamId)
        {
            if (streamId == null || string.IsNullOrWhiteSpace(streamId.Value))
                throw new InvalidOperationException("stream Id is required");
            var result = await _connection.ReadStreamEventsForwardAsync(streamId.Value, StreamPosition.Start, 1000,false);
            if(result != null && result.Status == SliceReadStatus.Success)
            {
                List<IEvent> res = GetEvents(result);
                return res;
            }
            else
            {
                throw new InvalidOperationException(string.Format("There no events with Stream identifer : {0}", streamId.Value));
            }
           
        }

        private static List<IEvent> GetEvents(StreamEventsSlice events)
        {
            var Result = new List<IEvent>();
            foreach (var x in events.Events)
            {
                var data = Helper.Parse(x.Event);
                if (data != null)
                {
                    Result.AddRange(data);
                   
                }
            }
            return Result;
        }



        public async Task Save(List<EventStoreStream> newEvents)
        {
            if (newEvents.Count == 0)
                throw new InvalidOperationException("There no new event to save");
            foreach(var @event in newEvents)
            {
                await _connection.AppendToStreamAsync(@event.Id, ExpectedVersion.Any, Helper.AsJson(@event));
            }
        }
    }
}
