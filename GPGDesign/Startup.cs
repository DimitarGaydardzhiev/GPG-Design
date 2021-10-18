using DataLayer;
using GPGDesign.Components;
using GPGDesign.Data;
using GPGDesign.Models;
using GPGDesign.Services;
using GPGDesign.Utils.GoogleAnalitycs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NToastNotify;
using NToastNotify.Libraries;
using System.Collections.Generic;
using System.Globalization;

namespace GPGDesign
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
            services.AddMvc();
            services.AddDbContext<GPGContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 6;
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<GPGContext>()
            .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.AddMvc()
                .AddViewLocalization(opts => { opts.ResourcesPath = "Resources"; })
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .AddNToastNotifyToastr(new ToastrOptions()
                {
                    ProgressBar = false,
                    PositionClass = ToastPositions.BottomRight
                }); ;

            var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("bg"),
                    new CultureInfo("en"),
                    // new CultureInfo("de")
                };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("bg");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                    {
                        new QueryStringRequestCultureProvider(),
                        new CookieRequestCultureProvider()
                    };
            });

            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            // Register the Google Analytics configuration
            services.Configure<GoogleAnalyticsOptions>(options => Configuration.GetSection("GoogleAnalytics").Bind(options));

            // Register the TagHelperComponent
            services.AddTransient<ITagHelperComponent, GoogleAnalyticsTagHelperComponent>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseFileServer();
            app.UseStaticFiles();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseNToastNotify();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
