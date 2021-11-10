using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApplication.Core.Abstractions;
using WebApplication.Core.Behaviors;
using WebApplication.Infrastructure.Data;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication.Web
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
            var assemblies = new[]
            {
                Assembly.Load("WebApplication.Core"),
                Assembly.Load("WebApplication.Infrastructure"),
                Assembly.Load("WebApplication.Web")
            };

            services
                .AddAutoMapper(assemblies)
                .AddMediatR(assemblies);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            foreach (var item in AssemblyScanner.FindValidatorsInAssemblies(assemblies))
                services.AddScoped(item.InterfaceType, item.ValidatorType);

            services
                .AddDbContext<WebApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WebApplication")))
                .AddTransient<IWebApplicationDbContext, WebApplicationDbContext>(provider => provider.GetService<WebApplicationDbContext>());

            services
                .AddAuthentication()
                .AddJwtBearer();

            services
                .AddControllers();

            services
                .AddSpaStaticFiles(configuration => configuration.RootPath = "App/dist");

            services
                .AddSwaggerGen(options =>
                {
                    options.CustomOperationIds(api => api.TryGetMethodInfo(out var method) ? method.Name : null);
                    options.DescribeAllParametersInCamelCase();
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "WebApplication API",
                        Version = "v1"
                    });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebApplicationDbContext db)
        {
            db.Database.Migrate();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // For development, Chrome requires secure cookies w/ http, so let's make it lax.
            if (env.IsDevelopment())
                app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
            else
                app.UseHttpsRedirection();

            app.UseExceptionHandler(scope =>
            {
                scope.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (exceptionHandlerPathFeature?.Error is ValidationException e)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = MediaTypeNames.Application.Json;

                        await context.Response.WriteAsJsonAsync(e.Errors.Select(error => error.ErrorMessage).ToList());
                    }
                });
            });

            app.UseStaticFiles();
            if (!env.IsDevelopment())
                app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet(".authority", async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = MediaTypeNames.Text.Plain;

                    await context.Response.WriteAsync(Configuration.GetValue<string>("Authority"));
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication API V1"));

            // Use a reverse proxy to the dev server to prevent CORS violations.
            // This pattern changes in ASP.NET 6.
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "App";

                // If using Docker, make sure to change the host name to the name of your dev server service name.
                if (env.IsDevelopment())
                    spa.UseProxyToSpaDevelopmentServer("http://webapplication.webapp:4200");
            });
        }
    }
}