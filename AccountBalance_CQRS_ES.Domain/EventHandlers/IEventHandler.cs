using AccountBalance_CQRS_ES.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.EventHandlers
{
    public interface IEventHandler<T> where T : IEvent
    {
        string Id { get;}
        List<T> Events { get; set; }
    }
}
