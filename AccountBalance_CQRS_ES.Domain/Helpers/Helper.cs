using AccountBalance_CQRS_ES.Domain.EventHandlers;
using AccountBalance_CQRS_ES.Domain.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Helpers
{
   public static class Helper
    {
        public static EventData AsJson(EventStoreStream value)
        {
            if (value == null) throw new ArgumentNullException("value");
            var json = JsonConvert.SerializeObject(value);
            var data = Encoding.UTF8.GetBytes(json);
            var eventName = value.Events.First().GetType().Name;
            return new EventData(Guid.NewGuid(), eventName, true, data, new byte[] { });
        }
        public static IEnumerable<IEvent> Parse(this RecordedEvent data)
        {
            if (data == null) throw new ArgumentNullException("data");
            var value = Encoding.UTF8.GetString(data.Data);
            if (CheckType(data.EventType,nameof(AccountCreated)))
            {

                return JsonConvert.DeserializeObject<CreateAccountHandler>(value).Events;

            }
            if (CheckType(data.EventType,nameof(DailyWireTransferLimitChanged)))
            {
                return JsonConvert.DeserializeObject<DailyWireTransferLimitChangedEventHandler>(value).Events;
            }
            if (CheckType(data.EventType, nameof(CashDeposited)))
            { return JsonConvert.DeserializeObject<CashDepositedHandler>(value).Events; }
            if(CheckType(data.EventType,nameof(OverdraftLimitChanged)))
            { return JsonConvert.DeserializeObject<OverdraftLimitChangedEventHandler>(value).Events; }
            if (CheckType(data.EventType, nameof(AccountBlocked)))
            {
                return JsonConvert.DeserializeObject<AccountBlockedEventHandler>(value).Events;
            }
            if (CheckType(data.EventType, nameof(CashWithdrawn)))
            {
                return JsonConvert.DeserializeObject<CashWithdrawnEventHandler>(value).Events;
            }
            if (CheckType(data.EventType, nameof(AccountUnblocked))){
                return JsonConvert.DeserializeObject<AccountUnblockedEventHandler>(value).Events;
            }
            return null;
        }
        private static bool CheckType(string StreamEventType ,string EventType)
        {
            if (StreamEventType.Equals(EventType)) return true;
            return false;

        }
    }
}
