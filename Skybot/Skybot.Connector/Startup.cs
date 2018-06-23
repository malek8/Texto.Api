using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skybot.Connector.Services;

namespace Skybot.Connector
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMessageService, MessageService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //var serviceProvider = services.BuildServiceProvider();
            //var messageService = serviceProvider.GetService<IMessageService>();
            //messageService.ProcessIncomingMessages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            var messageService = (IMessageService)app.ApplicationServices.GetService(typeof(IMessageService));
            messageService.ProcessIncomingMessages();
        }
    }
}
