using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.SignalR;
using Czeum.DAL;
using Czeum.Abstractions.GameServices;
using Czeum.ChessLogic;
using Czeum.Connect4Logic;
using Czeum.DAL.Entities;
using Czeum.Server.Hubs;
using Czeum.Server.Services;
using Czeum.Server.Services.FriendService;
using Czeum.Server.Services.GameHandler;
using Czeum.Server.Services.Lobby;
using Czeum.Server.Services.MessageService;
using Czeum.Server.Services.OnlineUsers;
using Czeum.Server.Services.ServiceContainer;
using Czeum.Server.Services.SoloQueue;
using IdentityModel;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Czeum.Server.Configurations;
using Czeum.Server.Services.EmailSender;
using Microsoft.AspNetCore.Identity.UI.Services;
using NSwag;

namespace Czeum.Server
{
    public class Startup {
        public Startup(IWebHostEnvironment env) 
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) 
        {
            services.Configure<CookiePolicyOptions>(options => 
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

			services.Configure<IdentityOptions>(options => 
            {
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireNonAlphanumeric = false;

				options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = false;
			});

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddAspNetIdentity<ApplicationUser>();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Authority"];
                    options.Audience = "czeum_api";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                    };
                });

            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson(protocol =>
                {
                    protocol.SerializerSettings.TypeNameHandling = TypeNameHandling.All;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSignalR()
                .AddJsonProtocol();
            
            services.AddSwaggerDocument(options =>
            {
                options.DocumentName = "Chitchat";
                options.Title = "Chitchat API";
                options.Version = "1.0";
                options.Description = "Web API for the world's simplest chat client.";

                options.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

            });

            //Singleton in memory storing services
			services.AddSingleton<ISoloQueueService, SoloQueueService>();
            services.AddSingleton<IOnlineUserTracker, OnlineUserTracker>();
			services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            
			//Game Services
            services.AddTransient<IGameService, Connect4Service>();
            services.AddTransient<IGameService, ChessService>();
            services.AddTransient<IGameHandler, GameHandler>();
            services.AddTransient<IServiceContainer, ServiceContainer>();
            
            //Lobby
            services.AddSingleton<ILobbyStorage, LobbyStorage>();
            services.AddTransient<ILobbyService, LobbyService>();

            services.AddTransient<IFriendService, FriendService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmailSender, EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
        {
            if (env.IsDevelopment()) 
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            } 
            else 
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseIdentityServer();
            app.UseAuthentication();
            
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseSignalR(route => route.MapHub<GameHub>("/gamehub"));

            app.UseMvc(routes => 
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
