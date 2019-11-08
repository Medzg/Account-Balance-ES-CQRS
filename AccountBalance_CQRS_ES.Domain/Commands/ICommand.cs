using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Commands
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
