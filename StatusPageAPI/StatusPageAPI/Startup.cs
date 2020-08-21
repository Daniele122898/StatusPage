using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StatusPageAPI.Extensions;
using StatusPageAPI.Helpers;
using StatusPageAPI.Models.Configurations;
using StatusPageAPI.Services;

namespace StatusPageAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        // For later use
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Status Page",
                    Version = "v1",
                    Description = "Status Page for different services"
                });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Used for JWT token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        }, 
                        new List<string>()
                    }
                });

                // Set the comments path for the Swagger Json and UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            
            this.ConfigureServices(services);
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var authSettings = Configuration.GetSection("Authentication").Get<AuthenticationConfig>();
            var jwtHelper = new JwtHelperService(authSettings);

            
            services.AddSingleton(jwtHelper);

            services.AddControllers();
            services.AddRouting(op => op.LowercaseUrls = true);
            services.AddCors();
            ServiceInjections.AddCustomServices(services);

            services.AddAuthentication()
                .AddJwtBearer("Bearer", op =>
                {
                    op.SaveToken = true;
                    op.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtHelper.JwtKey,
                        ValidateIssuer = true,
                        //Usually, this is your application base URL
                        ValidIssuer = authSettings.TokenIssuer,
                        ValidateAudience = false,
                        //Here, we are creating and using JWT within the same application.
                        //In this case, base URL is fine.
                        //If the JWT is created using a web service, then this would be the consumer URL.
                        // WE CANNOT USE THIS 
                        // we use this as an API that you can call via any other machines and processes. 
                        // ValidAudience = "http://localhost:5000/", 
                        RequireExpirationTime =
                            false, // So we can generate permanent tokens for easier API management
                        ValidateLifetime =
                            true, // We still want to validate the frontend tokens tho since those are time bound.
                        LifetimeValidator = LifetimeValidator
                    };
                });
            
            services.AddAuthorization(op =>
            {
                op.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Bearer").Build();
            });
            
            services.Configure<AuthenticationConfig>(Configuration.GetSection("Authentication"));
        }

        private bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            return notBefore <= DateTime.UtcNow && expires >= DateTime.UtcNow;
        }
                    
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StatusPage");
                });
            }
            else
            {
                app.UseExceptionHandler(b =>
                {
                    b.Run(async context =>
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                        var err = context.Features.Get<IExceptionHandlerFeature>();
                        if (err != null)
                        {
                            context.Response.AddApplicationError(err.Error.Message);
                            await context.Response.WriteAsync(err.Error.Message);
                        }
                    });
                });
            }
            
            // Warmup services
            app.ApplicationServices.GetRequiredService<EntityCheckService>();
            
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}