using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Application.IServices;
using Virgo.DependencyInjection;

namespace Virgo.Application.Services
{
    
    public class CustomService : ICustomService, ITransientDependency
    {
        public string Call()
        {
            return "Service Call";
        }
    }
}
