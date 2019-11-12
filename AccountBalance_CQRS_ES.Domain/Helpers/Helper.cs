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
        public static DateTime GetNextBusinessDay(this DateTime date)
        {
            TimeSpan ts = new TimeSpan(09, 00, 0);
            if (date.DayOfWeek >= DayOfWeek.Monday && date.DayOfWeek < DayOfWeek.Friday
               && date.Hour >= DateTime.Parse("09:00").Hour &&
               date.Hour <= DateTime.Parse("17:00").Hour)
            {
                DateTime bday = DateTime.UtcNow.AddDays(1).Date + ts;
                return date + bday.Subtract(date);
            }

            if (date.DayOfWeek == DayOfWeek.Friday
               && date.Hour >= DateTime.Parse("09:00").Hour && date.Hour <= DateTime.Parse("17:00").Hour)
                return DateTime.UtcNow.AddDays(3).Date + ts;
            if (date.DayOfWeek == DayOfWeek.Saturday)
                return DateTime.UtcNow.AddDays(2).Date + ts;
            else

                return DateTime.UtcNow.AddDays(1).Date + ts;
        }
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
            switch (data.EventType)
            {
                case nameof(AccountCreated): return JsonConvert.DeserializeObject<AccountCreatedEventHandler>(value).Events;
                case nameof(DailyWireTransferLimitChanged): return JsonConvert.DeserializeObject<DailyWireTransferLimitChangedEventHandler>(value).Events;
                case nameof(CashDeposited): return JsonConvert.DeserializeObject<CashDepositedHandler>(value).Events;
                case nameof(OverdraftLimitChanged): return JsonConvert.DeserializeObject<OverdraftLimitChangedEventHandler>(value).Events;
                case nameof(AccountBlocked): return JsonConvert.DeserializeObject<AccountBlockedEventHandler>(value).Events;
                case nameof(CashWithdrawn): return JsonConvert.DeserializeObject<CashWithdrawnEventHandler>(value).Events;
                case nameof(AccountUnblocked): return JsonConvert.DeserializeObject<AccountUnblockedEventHandler>(value).Events;
                case nameof(WireTransfered): return JsonConvert.DeserializeObject<WireTransferedEventHandler>(value).Events;
                case nameof(ChequeDeposited): return JsonConvert.DeserializeObject<ChequeDepositedEventHandler>(value).Events;
                default: return null;
                    
            }
          
            
           
  
          
         
       
        }
     
    }


   
}
