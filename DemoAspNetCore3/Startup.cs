using AutoMapper;
using Calzolari.Grpc.AspNetCore.FluentValidation;
using DemoAspNetCore3.Services;
using DemoGrpc.Repository;
using DemoGrpc.Repository.Database;
using DemoGrpc.Repository.Interfaces;
using DemoGrpc.Web.Logging;
using DemoGrpc.Web.Services;
using DemoGrpc.Web.Validator;
using DempGrpc.Services;
using DempGrpc.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace DemoAspNetCore3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://login.microsoftonline.com/136544d9-038e-4646-afff-10accb370679";
                options.Audience = "257b6c36-1168-4aac-be93-6f2cd81cec43";
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization();

            services.AddDbContext<DemoDbContext>(options => options.UseSqlServer(Configuration["MySecretConnectionString"]));

            services.AddGrpc(options =>
            {
                options.EnableMessageValidation();
                options.Interceptors.Add<LoggerInterceptor>();
            });

            services.AddValidator<CountryCreateRequestValidator>();

            services.AddGrpcValidation();

            services.AddAutoMapper(Assembly.Load("DemoGrpc.Web"));

            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICountryRepository, EFCountryRepository>();

            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseGrpcWeb();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMiddleware<CustomExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MyOwnGRpcService>();
                endpoints.MapGrpcService<CountryGrpcService>();//.EnableGrpcWeb();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
