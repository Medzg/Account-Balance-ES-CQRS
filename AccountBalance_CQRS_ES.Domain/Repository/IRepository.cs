using AccountBalance_CQRS_ES.Domain.Aggregate;
using AccountBalance_CQRS_ES.Domain.EStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.Domain.Repository
{
    public interface IRepository
    {
   
        Task<T> GetById<T>(Guid Id) where T : AggregateRoot, new();

        Task Save(params AggregateRoot[] streamItems);
    }
}
