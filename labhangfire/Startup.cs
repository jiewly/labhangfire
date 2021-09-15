using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace labhangfire
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
            services.AddHangfire(config => { config.UseSqlServerStorage("Server=(localdb)\\MSSQLLocalDB;Database=HangfireDemo;User Id=sa;Password=P@ssw0rd"); });
            services.AddHangfireServer();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseHangfireDashboard();

            backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            //var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Job Delayed !"), TimeSpan.FromMinutes(1));
            // BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Job Continue!"));
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring!"), Cron.Minutely); // Daily, Minutely, Hourly, Weekly
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
