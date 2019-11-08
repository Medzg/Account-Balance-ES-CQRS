using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Helpers
{
    public class StreamIdentifier
    {
        public StreamIdentifier(string name , Guid id)
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Stream identifier : name required");
            }
            if (id == null || id == Guid.Empty)
            {
                throw new InvalidOperationException("Stream identifier : id required");
            }
            this.Value = string.Format("{0}-{1}", name, id.ToString());

        }
        public string Value { get; private set; }
    }
}
