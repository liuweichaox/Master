using System;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Extras.IocManager;
using Microsoft.AspNetCore.Hosting.Server;
using IModule = Autofac.Extras.IocManager.IModule;

namespace Virgo.Presentation
{
    public class PresentationModule : IModule
    {
        public void Register(IIocBuilder iocBuilder)
        {
            iocBuilder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
        }
    }
}