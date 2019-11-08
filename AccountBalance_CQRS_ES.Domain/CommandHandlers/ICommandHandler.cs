using AccountBalance_CQRS_ES.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.CommandHandlers
{
     public  interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand cmd);
    }
}
