using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestJWKSServer
{
    public class Startup
    {
        /// <summary>
        /// Constructor to initialize configuration
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration property to access app settings
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// Configures services for dependency injection
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC services
            services.AddControllers();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// Configures middleware pipeline
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Check environment to enable developer exception page
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable routing
            app.UseRouting();

            // Configure endpoints
            app.UseEndpoints(endpoints =>
            {
                // Map controller endpoints
                endpoints.MapControllers();
            });
        }
    }
}
