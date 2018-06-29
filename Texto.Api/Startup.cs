using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Texto.Api.Services;
using Texto.Data;

namespace Texto.Api
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
            services.AddTransient<IContextSettings, ContextSettings>();
            services.AddTransient<IContactsContext, ContactsContext>();
            services.AddTransient<IContactsService, ContactsService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IBusService, BusService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }

    public class ContextSettings : IContextSettings
    {
        private readonly IConfiguration configuration;

        public ContextSettings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string ConnectionString => configuration["Database:ConnectionString"];
        public string DatabaseName => configuration["Database:Name"];
        public string CollectionName => "Contacts";
    }
}
