using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Cli.Commands.Shared
{
    static class ServiceHost
    {
        static public IServiceProvider ServiceProvider { get; set; }

        static public void Init(Action<IServiceCollection> configServices)
        {
            if(ServiceProvider == null)
            {
                IServiceCollection serviceCollection = new ServiceCollection();
                configServices(serviceCollection);
                ServiceProvider = serviceCollection.BuildServiceProvider();
            }
        }
    }
}
