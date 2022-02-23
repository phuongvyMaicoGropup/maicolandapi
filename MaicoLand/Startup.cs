using MaicoLand.Models;
using MaicoLand.Repositories;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Builder;
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
using System;
using System.Collections.Generic;
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
            var mongoDbSettings = Configuration.GetSection(nameof(MaicoLandDatabaseSettings)).Get<MaicoLandDatabaseSettings>();

            services.AddTransient<INewsRepository, NewsRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();


            // requires using Microsoft.Extensions.Options
            services.Configure<MaicoLandDatabaseSettings>(
                Configuration.GetSection(nameof(MaicoLandDatabaseSettings)));

            //services.AddSingleton<IMaicoLandDatabaseSettings>(sp =>
            //    sp.GetRequiredService<IOptions<MaicoLandDatabaseSettings>>().Value);
            services.AddIdentity<AppUser, AppRole>()
        .AddMongoDbStores<AppUser, AppRole, Guid>
        (
        //"mongodb://localhost:27017", "MaicoLand"
        Configuration.GetConnectionString("DefaultConnection"), Configuration.GetConnectionString("DatabaseName")
        );



            // Enable   CORs

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            //Json Serializer
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MaicoLand", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Enable Cors
            app.UseCors(options => options.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MaicoLand v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
