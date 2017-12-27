using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Mashi.Models;
using Telegram.Bot.Mashi.Services;

namespace Telegram.Bot.Mashi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
			var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<BotData>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("BotDataConnection"));
            });
            services.AddScoped<IUpdateService, UpdateService>();
			services.AddSingleton<IBotService, BotService>();
            services.AddSingleton<IGlobalService, GlobalService>();

            services.AddSingleton(Configuration.GetSection("BotConfiguration").Get<BotConfiguration>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, BotData botData)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            botData.Database.EnsureCreated();
            app.UseMvc();
        }
    }
}
