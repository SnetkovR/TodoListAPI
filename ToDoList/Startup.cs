using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Users;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;
using ToDoList.JWT;
using ToDoList.Services;

namespace ToDoList
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
            var signingSecurityKey = Configuration.GetSection("SecretKey").Value;
            var signingKey = new SigningSymmetricKey(signingSecurityKey);
            services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

            services.AddScoped<TodoService>();
            services.AddScoped<UserRepository>();

            const string jwtSchemeName = "JwtBearer";
            var signingDecodingKey = (IJwtSigningDecodingKey)signingKey;
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddAuthentication()
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.RequireHttpsMetadata = true;

                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingDecodingKey.GetKey(),

                        ValidateIssuer = false,
                        ValidIssuer = "ToDoList",

                        ValidateAudience = false,
                        ValidAudience = "Client",

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Test API",
                    Description = "ASP.NET Core Web API"
                });
            });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });
        }
    }
}
