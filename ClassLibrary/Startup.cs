using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;

namespace DeviceApi
{
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
                    SecurityKey rsa = services
                        .BuildServiceProvider()
                        .GetRequiredService<RsaSecurityKey>();
                    options.IncludeErrorDetails = true;
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = rsa,
                        ValidAudience = "organization",
                        ValidIssuer = "luneau-auth-server",
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var path = context.HttpContext.Request.Path;
                            if (!path.StartsWithSegments("/hubs"))
                                return Task.CompletedTask;
                            var accessToken = context.Request.Query["access_token"];
                            context.Token = accessToken;
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuthentication(services);
            services.AddSingleton<HubService>();
            services.AddSignalR();
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.ApplicationServices.GetService(typeof(HubService));
            app.UseSignalR(route =>
            {
                route.MapHub<DeviceHub>("/hubs");
            });
            app.UseCors(builder =>
            {
                builder
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMvc();
        }
    }
}
