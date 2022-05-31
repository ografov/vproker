using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using vproker.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Threading;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using vproker.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;
using Swashbuckle.AspNetCore.Swagger;

namespace vproker
{
    public class Startup
    {
        private const string CORS_POLICY_NAME = "allowAll";

        public Startup(IConfiguration configuration, IHostingEnvironment hosting)
        {
            Configuration = configuration;
            HostingEnvironment = hosting;
        }

        public IHostingEnvironment HostingEnvironment { get; set; }
        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure CORS
            ConfigureCors(services);

            services.AddEntityFrameworkSqlite()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(
                        new SqliteConnectionStringBuilder { DataSource = $"vproker.db" }.ToString()));

            var passwordOptions = new PasswordOptions()
            {
                RequireDigit = false,
                RequireNonAlphanumeric = false,
                RequiredLength = 6,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.Password = passwordOptions)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<OrderService>();
            services.AddTransient<ClientService>();
            services.AddTransient<MaintainService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "vrpoker API",
                    Description = "vproker Web API",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Oleg Grafov", Email = "olegraf@gmail.com" }
                });
            });

            services.AddAntiforgery();
            services.AddMvc();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            ApplicationDbContext context)
        {
            // setting Ru culture helped for showing right currency, but does not for DateTime
            var cultureInfo = new CultureInfo("ru-RU");
            cultureInfo.NumberFormat.CurrencyDecimalDigits = 0;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            // don't see a diff, but leaving the following lines too
            var localizationOptions = new RequestLocalizationOptions()
            {
                SupportedCultures = new List<CultureInfo>() { cultureInfo },
                SupportedUICultures = new List<CultureInfo>() { cultureInfo },
                DefaultRequestCulture = new RequestCulture(cultureInfo),
                FallBackToParentCultures = false,
                FallBackToParentUICultures = false,
                RequestCultureProviders = null
            };
            app.UseRequestLocalization(localizationOptions);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

            Migrate(app);
            //app.UseIISPlatformHandler(); // options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();
            app.UseCors(CORS_POLICY_NAME);

            app.UseAuthentication();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Order}/{action=ActiveOrders}/{id?}");
            });

            SampleData.Initialize(app.ApplicationServices);

            //ExtractClients.Process(app.ApplicationServices);

            //ReadClients.ReadFromFile(app.ApplicationServices);

            //PassportCheck.LimitExpiredPassports("36");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            AuthData.SeedAuth(app.ApplicationServices).Wait();
        }

        private void ConfigureCors(IServiceCollection services)
        {
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy(CORS_POLICY_NAME, corsBuilder.Build());
            });
        }

        private void Migrate(IApplicationBuilder app)
        {
            // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                         .Database.Migrate();
                }
            }
            catch { }
        }
    }
}
