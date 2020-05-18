using Autofac;
using AutoMapper;
using Czeum.Application.Services;
using Czeum.Application.Services.Lobby;
using Czeum.ChessLogic.Services;
using Czeum.Connect4Logic.Services;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.Domain.Entities;
using Czeum.Domain.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using System;
using System.Linq;
using System.Reflection;
using Czeum.Web.AutofacModules;
using Czeum.Web.Extensions;
using Czeum.Web.IdentityServer;
using Czeum.Web.Middlewares;
using Czeum.Web.Services;
using Czeum.Web.SignalR;
using IdentityServer4;
using System.Threading.Tasks;
using Czeum.Core.GameServices;
using Czeum.Core.GameServices.ServiceMappings;

namespace Czeum.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<CzeumContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        public virtual void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["IdentityServer:Authority"];
                    options.Audience = "czeum_api";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name
                    };

                    // SignalR JS Client
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(token) && context.Request.Path.StartsWithSegments("/notifications"))
                            {
                                context.Token = token;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public virtual void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;

                //options.SignIn.RequireConfirmedEmail = true;
            });

            ConfigureDatabase(services);
            
            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<CzeumContext>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClientsFromConfiguration(Configuration))
                .AddCorsPolicyService<CorsPolicyService>()
                .AddAspNetIdentity<User>();

            ConfigureControllers(services);

            services.AddRazorPages();

            ConfigureAuthentication(services);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MyPolicy", options => options.RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

                options.AddPolicy("MyCookiePolicy", options => options.RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme));

                options.DefaultPolicy = options.GetPolicy("MyPolicy");
                options.InvokeHandlersAfterFailure = false;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
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
            services.AddTransient<IUserIdProvider, UserIdProvider>();
            services.AddTransient<INotificationService, NotificationService>();

            services.AddAutoMapper(Assembly.Load("Czeum.Application"));

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist/czeum-client-app";
            });

            services.AddMvc().AddNewtonsoftJson();

            services.AddSingleton<IGameTypeMapping>(provider => GameTypeMapping.Instance);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ServiceModule>();

            builder.RegisterGame<Connect4BoardCreator, Connect4BoardConverter, Connect4MoveHandler>("Connect4")
                .RegisterGame<ChessBoardCreator, ChessBoardConverter, ChessMoveHandler>("Sakk");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CzeumContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                context.Database.Migrate();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseIdentityServer();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHub<NotificationHub>("/notifications");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
