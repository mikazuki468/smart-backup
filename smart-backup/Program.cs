using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using smart_backup.Service;
using System;
using System.Diagnostics;
using System.IO;

namespace smart_backup
{
    public class Program
    {
        static void Main(string[] args)
        {
            #region aggiungo Ilogger
            var services = new ServiceCollection();
            ConfigureServices(services);
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                Business app = serviceProvider.GetService<Business>();
                // Start up logic here
                app.Run();
                #endregion
            }
        }

            private static void ConfigureServices(ServiceCollection services)
            {
                services.AddLogging(configure => configure.AddConsole())
                .AddTransient<Business>();
            }
        
    }
}
