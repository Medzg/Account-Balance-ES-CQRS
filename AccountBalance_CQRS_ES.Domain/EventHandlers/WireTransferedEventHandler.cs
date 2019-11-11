﻿using AccountBalance_CQRS_ES.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.EventHandlers
{
    public class WireTransferedEventHandler : IEventHandler<WireTransfered>
    {
        public string Id { get; set; }

        public List<WireTransfered> Events { get; set; }
    }
}
