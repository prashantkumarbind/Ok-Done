using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

using Microsoft.Extensions.FileProviders;
using System.IO;
using eSanjeevaniIcu.Portal.Controllers;
using Microsoft.Extensions.Hosting;
using eSanjeevaniIcu.Portal.Controllers;
using eSanjeevaniIcu.Portal.Hubs;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Hosting.Server;

namespace eSanjeevaniIcu.Portal
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSignalR();
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.Configure<ApplicationConfigurations>
      (Configuration.GetSection("ApplicationConfigurations"));

            services.Configure<EmailConfiguration>
      (Configuration.GetSection("EmailConfiguration"));

            //services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                
            });
            
            services.AddHttpContextAccessor();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddDbContext<eSanjeevaniIcuDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("objeSanjeevaniIcuDbCon"), x => x.UseNetTopologySuite()));
            services.Configure<ApplicationConfigurations>(Configuration.GetSection("ApplicationConfigurations"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors("MyPolicy");
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
                //endpoints.MapHub<NotificationHub>("/notificationHub");
                string HubUrl = Configuration.GetSection("ApplicationConfigurations:ApplicationNotificationHub").Value.ToString();
                endpoints.MapHub<NotificationHub>(HubUrl);
            });
        }
    }
}

