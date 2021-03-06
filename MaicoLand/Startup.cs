//WI9AFNgbYFVV
using MaicoLand.Models;
using MaicoLand.Models.Entities;
using MaicoLand.Models.StructureType;
using MaicoLand.Repositories;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
            });
            

            services.AddRazorPages();
            services.AddControllers();
            var mongoDbSettings = Configuration.GetSection(nameof(MaicoLandDatabaseSettings)).Get<MaicoLandDatabaseSettings>();
            services.AddTransient<IPostRepository<News>, NewsRepository>();
            services.AddTransient<IPostRepository<SalePost>, SalePostRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<ILandPlanningRepository, LandPlanningRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddTransient<ISendMailService, SendMailService>();
           
            
            services.Configure<MaicoLandDatabaseSettings>(Configuration.GetSection(nameof(MaicoLandDatabaseSettings)));

            services.AddSingleton<IMaicoLandDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<MaicoLandDatabaseSettings>>().Value);


            services.AddOptions();                                         // K??ch ho???t Options
            var mailsettings = Configuration.GetSection("MailSettings");  // ?????c config
            services.Configure<MailSettings>(mailsettings);
            services.AddIdentity<AppUser, AppRole>()
                .AddMongoDbStores<AppUser, AppRole, Guid>(mongoDbSettings.ConnectionString, mongoDbSettings.DatabaseName)
                .AddDefaultTokenProviders()
               ;
            //services.AddAuthentication().Add

            // Enable   CORs

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@".\safespot")); 
    //        Services.AddDataProtection()
    //.SetDefaultKeyLifetime(TimeSpan.FromDays(14));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MaicoLand", Version = "v1" });
            });

            // Add services to the container
            //var logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Enable Cors
            app.UseCors(options => options.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());

            
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MaicoLand v1"));
            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
