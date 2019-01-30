using Microsoft.ApplicationInsights.ServiceFabric;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Virgo.Domain.Uow
{
    internal class UnitOfWorkDefaultOptions : IUnitOfWorkDefaultOptions
    {
        public TransactionScopeOption Scope { get; set; }

        public bool IsTransactional { get; set; }

        public TimeSpan? Timeout { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }
        public UnitOfWorkDefaultOptions()
        {
            IsTransactional = true;
            Scope = TransactionScopeOption.Required;
        }
    }
}
