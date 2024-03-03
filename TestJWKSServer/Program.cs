using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TestJWKSServer
{
    public class Program
    {
        /// <summary> 
        /// Entry point of the application
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Build and run the host
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Configures the host and initializes the startup class
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Use Startup class to configure the application
                    webBuilder.UseStartup<Startup>();
                });
    }
}
