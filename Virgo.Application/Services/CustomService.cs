using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Application.Interfaces;
using Virgo.DependencyInjection;

namespace Virgo.Application.Services
{
    public class CustomService : ICustomService, ITransientDependency
    {
        public bool Call()
        {
            System.Diagnostics.Debug.WriteLine("Service Calling");
            return true;
        }
    }
}
