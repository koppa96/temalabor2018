using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Czeum.DAL;
using System.Text;
using Czeum.Abstractions;
using Czeum.Abstractions.GameServices;
using Czeum.ChessLogic;
using Czeum.Connect4Logic;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.DAL.Repositories;
using Czeum.Server.Hubs;
using Czeum.Server.Services;
using Czeum.Server.Services.Lobby;
using Czeum.Server.Services.OnlineUsers;
using Czeum.Server.Services.ServiceContainer;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace Czeum.Server
{
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

			services.Configure<IdentityOptions>(options => {
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireNonAlphanumeric = false;

				options.User.RequireUniqueEmail = true;
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
                .AddJwtBearer(options => {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters() {
                    ValidateIssuer = true,
                    ValidIssuer = "Czeum.Server",
                    ValidateAudience = true,
                    ValidAudience = "Czeum.Server",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Connect4SecureSigningKey"))
                };
            });

            services.AddMvc()
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSignalR()
                .AddNewtonsoftJsonProtocol(protocol =>
                {
                    protocol.PayloadSerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                });

			services.AddSingleton<ISoloQueueService, SoloQueueService>();
            services.AddSingleton<IOnlineUserTracker, OnlineUserTracker>();
			services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddTransient<IServiceContainer, ServiceContainer>();
            
			//Game Services
            services.AddScoped<IGameService, Connect4Service>();
            services.AddScoped<IGameService, ChessService>();
            
            //Lobby
            services.AddSingleton<ILobbyStorage, LobbyStorage>();
            services.AddScoped<ILobbyService, LobbyService>();
            
            //Repositories
            services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IBoardRepository<SerializedBoard>, BoardRepository<SerializedBoard>>();
            services.AddScoped<IBoardRepository<SerializedChessBoard>, BoardRepository<SerializedChessBoard>>();
            services.AddScoped<IBoardRepository<SerializedConnect4Board>, BoardRepository<SerializedConnect4Board>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSignalR(route => { route.MapHub<GameHub>("/gamehub"); });

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
