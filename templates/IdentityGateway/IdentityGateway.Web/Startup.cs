using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using IdentityGateway.Core.Abstractions;
using IdentityGateway.Core.Behaviors;
using IdentityGateway.Infrastructure.Data;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IdentityGateway.Web
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
                Assembly.Load("IdentityGateway.Core"),
                Assembly.Load("IdentityGateway.Infrastructure"),
                Assembly.Load("IdentityGateway.Web")
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
                .AddDbContext<IdentityGatewayDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityGateway")))
                .AddTransient<IIdentityGatewayDbContext, IdentityGatewayDbContext>(provider => provider.GetService<IdentityGatewayDbContext>());

            services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityGatewayDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "/login";
                    options.UserInteraction.LogoutUrl = "/logout";
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddApiAuthorization<IdentityUser, IdentityGatewayDbContext>();

            services
                .AddAuthentication()
                .AddIdentityServerJwt();

            services
                .AddOidcStateDataFormatterCache()
                .AddDistributedMemoryCache();

            services
                .AddControllers();

            services
                .AddSpaStaticFiles(configuration => configuration.RootPath = "dist");

            services
                .AddSwaggerGen(options =>
                {
                    options.CustomOperationIds(api => api.TryGetMethodInfo(out var method) ? method.Name : null);
                    options.DescribeAllParametersInCamelCase();
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "IdentityGateway API",
                        Version = "v1"
                    });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IdentityGatewayDbContext db)
        {
            db.Database.Migrate();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // for development only
            // chrome requires secure cookies w/ http, so let's make it lax
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

                        // TODO await context.Response.WriteAsJsonAsync(e.Errors.Select(error => error.ErrorMessage).ToList());
                        await context.Response.WriteJsonAsync(JsonConvert.SerializeObject(e.Errors.Select(error => error.ErrorMessage).ToList()));
                    }
                });
            });

            app.UseStaticFiles();
            if (!env.IsDevelopment())
                app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityGateway API V1"));

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "./";

                if (env.IsDevelopment())
                    spa.UseProxyToSpaDevelopmentServer("http://identitygateway.webapp:4200");
            });
        }
    }
}