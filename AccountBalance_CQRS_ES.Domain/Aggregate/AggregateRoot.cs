using AccountBalance_CQRS_ES.Domain.Events;
using AccountBalance_CQRS_ES.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Aggregate
{
    public abstract class AggregateRoot
    {
        private readonly List<Event> _changes;
        protected Guid Id { get; set; }
       
        protected Dictionary<Type, Action<IEvent>> eventAppliers;

        public StreamIdentifier StreamIdentifier
        {
            get
            {
                return new StreamIdentifier(this.GetType().Name, this.Id);
            }
        }
        protected AggregateRoot()
        {
            this._changes = new List<Event>();
            this.eventAppliers = new Dictionary<Type, Action<IEvent>>();
            this.RegisterAppliers();
        }
        private void Apply(IEvent evt)
        {
            var evtType = evt.GetType();
            if (!this.eventAppliers.ContainsKey(evtType))
            {
                string message = String.Format("this event {0} is not regisitred in {1}", evt.GetType().Name, this);
                throw new InvalidOperationException(message);
            }
            this.eventAppliers[evtType](evt);
        }
        protected abstract void RegisterAppliers();
        protected void ApplyChanges(Event evt)
        {
            this.Apply(evt);
            this._changes.Add(evt);
        }
        protected void RegisterAppliers<TEvent>(Action<TEvent> applier) where TEvent : IEvent
        {
            this.eventAppliers.Add(typeof(TEvent), (x) => applier((TEvent)x));
        }
        public void LoadFromHistory(IEnumerable<IEvent> history)
        {

            foreach (var evt in history)
            {
                this.Apply(evt);
            }
        }
        public IEnumerable<Event> GetUncommittedChanges()
        {
            return _changes.AsReadOnly();
        }
        public void Commit()
        {
            this._changes.Clear();
        }
    }
}
