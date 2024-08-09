using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiPOC.DataBaseModel;
using WebApiPOC.Middleware;
using WebApiPOC.OAuth;
using WebApiPOC.OAuth.Interface;
using WebApiPOC.Services;
using WebApiPOC.Services.IServices;

namespace WebApiPOC
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
            var connection = Configuration["ConnectionString"];
            services.AddDbContext<transactionDBContext>(options =>
                    options.UseSqlServer(connection));

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<transactionDBContext>(options =>
                    options.UseSqlServer(connection));

            // for identityUser & IdentityRole use pkg Microsoft.AspNetCore.Identity.EntityFrameworkCore
            services.AddIdentity<User, UserRole>()
            .AddEntityFrameworkStores<transactionDBContext>()
            .AddDefaultTokenProviders();

            services.AddControllers();
            services.AddMemoryCache();// implementation of In-Memory caching

            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenManager, JWTTokenManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiPOC", Version = "v1" });
            });

            // =======================Configure JWT authentication=========================
            // add add package Microsoft.AspNetCore.Authentication.JwtBearer
            //add package Microsoft.IdentityModel.Tokens
            var key = Encoding.ASCII.GetBytes("WebApITest"); // Use a secure key
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiPOC v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Add authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // Register the custom exception middleware
            app.UseMiddleware<CustomExceptionMiddleware>();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
