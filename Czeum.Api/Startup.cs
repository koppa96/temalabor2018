using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Czeum.Api.AutofacModules;
using Czeum.Api.Extensions;
using Czeum.Api.IdentityServer;
using Czeum.Api.Middlewares;
using Czeum.Api.Services;
using Czeum.Api.SignalR;
using Czeum.Application.Services;
using Czeum.Application.Services.Lobby;
using Czeum.Application.Services.OnlineUsers;
using Czeum.ChessLogic.Services;
using Czeum.Connect4Logic.Services;
using Czeum.DAL;
using Czeum.Domain.Entities;
using Czeum.Domain.Services;
using IdentityModel;
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
using Newtonsoft.Json;
using NSwag;
using NSwag.AspNetCore;

namespace Czeum.Api
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
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;

                //options.SignIn.RequireConfirmedEmail = true;
            });

            services.AddDbContext<CzeumContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<CzeumContext>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddCorsPolicyService<CorsPolicyService>()
                .AddAspNetIdentity<User>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddRazorPages();

            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.Audience = "czeum_api";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MyPolicy", options => options.RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

                options.DefaultPolicy = options.GetPolicy("MyPolicy");
                options.InvokeHandlersAfterFailure = false;
            });

            services.AddSwaggerDocument(options =>
            {
                options.DocumentName = "Czeum";
                options.Title = "Czeum API";
                options.Version = "1.0";
                options.Description = "Web api for a server created to play board games.";

                options.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });
            });

            services.AddSignalR()
                .AddNewtonsoftJsonProtocol();

            services.AddSingleton<ILobbyStorage, LobbyStorage>();
            services.AddSingleton<IOnlineUserTracker, OnlineUserTracker>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddAutoMapper(Assembly.Load("Czeum.Application"));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ServiceModule>();

            builder.RegisterGame<Connect4BoardCreator, Connect4BoardConverter, Connect4MoveHandler>()
                .RegisterGame<ChessBoardCreator, ChessBoardConverter, ChessMoveHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CzeumContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                context.Database.Migrate();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCors();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseIdentityServer();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(routes =>
            {
                routes.MapRazorPages();
                routes.MapControllers();
                routes.MapHub<NotificationHub>("/notifications");
            });
        }
    }
}