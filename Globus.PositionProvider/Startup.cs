using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Globus.PositionProvider.SelfData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Globus.PositionProvider
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            SelfAircraft.Aircraft =
                new Aircraft()
                {
                    CallSign = "King",
                    TrueTrack = 37.2,
                    Altitude = 2000,
                    Position =
                        new Position()
                        { Lontitude = 34.82141, Latitude = 32.08374 }
                };

            new Thread(() =>
                {
                    while (true)
                    {
                        SelfAircraft.Aircraft.Position.Latitude =
                            SelfAircraft.Aircraft.Position.Latitude + 0.0001;
                        Thread.Sleep(33);
                    }
                }).Start();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors(options =>
                {
                    options
                        .AddPolicy(name: "CorsPolicy",
                        policy =>
                        {
                            policy.WithOrigins("http://localhost:3000");
                        });
                });

            services.AddControllers();
            services
                .AddSwaggerGen(c =>
                {
                    c
                        .SwaggerDoc("v1",
                        new OpenApiInfo {
                            Title = "Globus.PositionProvider",
                            Version = "v1"
                        });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app
                    .UseSwaggerUI(c =>
                        c
                            .SwaggerEndpoint("/swagger/v1/swagger.json",
                            "Globus.PositionProvider v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
