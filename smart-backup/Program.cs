using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartBackup.service;
using System;

namespace SmartBackup
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                Business app = serviceProvider.GetService<Business>();
                // Start up logic here
                app.Run();
            }
        }
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole())
            .AddTransient<Business>();
        }
    }
}
