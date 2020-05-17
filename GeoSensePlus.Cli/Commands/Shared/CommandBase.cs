using System;

namespace GeoSensePlus.Cli.Commands.Shared
{
    public class CommandBase
    {
        protected IServiceProvider sp;

        public CommandBase()
        {
            ServiceHost.Init(Program.ConfigureServices);
            sp = ServiceHost.ServiceProvider;
        }

        protected void Execute(Action fn)
        {
            try
            {
                fn();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
