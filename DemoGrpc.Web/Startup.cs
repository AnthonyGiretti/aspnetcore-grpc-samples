using AutoMapper;
using Calzolari.Grpc.AspNetCore.Validation;
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
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using CountryGrpcServiceV1 = DemoGrpc.Web.Services.V1.CountryGrpcService;
using System.Linq;
using System.Net;

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
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    options.Authority = "https://login.microsoftonline.com/136544d9-038e-4646-afff-10accb370679";
            //    options.Audience = "257b6c36-1168-4aac-be93-6f2cd81cec43";
            //    options.TokenValidationParameters.ValidateLifetime = true;
            //    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            //});

            //services.AddAuthorization();

            services.AddDbContext<DemoDbContext>(options => options.UseInMemoryDatabase(databaseName: "country_db"));

            var serviceProvider = services.BuildServiceProvider();
            using (var context = new DemoDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<DemoDbContext>>()))
            {
                context.Country.AddRange(
                    new DemoGrpc.Domain.Entities.Country
                    {
                        CountryId = 1,
                        CountryName = "Canada",
                        Description = "Maple leaf country"
                    },
                    new DemoGrpc.Domain.Entities.Country
                    {
                        CountryId = 2,
                        CountryName = "Japon",
                        Description = "Rising sun country"
                    },
                    new DemoGrpc.Domain.Entities.Country
                    {
                        CountryId = 3,
                        CountryName = "Australia",
                        Description = "Wallabies country"
                    });

                context.SaveChanges();
            }

            services.AddGrpc(options =>
            {
                options.EnableMessageValidation();
                options.Interceptors.Add<LoggerInterceptor>();
            });

            services.AddCors(o =>
            {
                o.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.WithExposedHeaders("Grpc-Status", "Grpc-Message");
                });
            });

            services.AddValidator<CountryCreateRequestValidator>();

            services.AddGrpcValidation();

            services.AddAutoMapper(Assembly.Load("DemoGrpc.Web"));

            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICountryRepository, EFCountryRepository>();

            //services.AddApplicationInsightsTelemetry();

            services.AddSingleton<ProtoService>();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseGrpcWeb();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                var protoService = endpoints.ServiceProvider.GetRequiredService<ProtoService>();

                endpoints.MapGrpcService<CountryGrpcServiceV1>().RequireCors("MyPolicy").EnableGrpcWeb();

                endpoints.MapGet("/protos", async context =>
                {
                    await context.Response.WriteAsync(await protoService.Get());
                });

                endpoints.MapGet("/protos/v{version:int}/{protoName}", async context =>
                {
                    var version = int.Parse((string)context.Request.RouteValues["version"]);
                    var protoName = (string)context.Request.RouteValues["protoName"];

                    var filePath = protoService.Get(version, protoName);

                    if (filePath != null)
                    {
                        await context.Response.SendFileAsync(filePath);
                    }
                    else
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    }
                });
            });
        }
    }
}