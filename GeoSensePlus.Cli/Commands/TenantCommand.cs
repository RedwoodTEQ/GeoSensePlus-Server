using GeoSensePlus.Cli.Commands.Shared;
using GeoSensePlus.Firestore.ConfigUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace GeoSensePlus.Cli.Commands
{
    public class TenantCommand : CommandBase
    {
        IConfigOperator _configOperator;

        public TenantCommand()
        {
            _configOperator = sp.GetService<IConfigOperator>();
        }

        public void Default()
        {
            Console.WriteLine("Tenant ID:");
            Console.WriteLine( _configOperator.GetTenant());
        }

        public void Set(string tenantName)
        {
            try
            {
                Console.WriteLine($"subcommand: tenant set {tenantName}");
                _configOperator.SetTenant(tenantName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
