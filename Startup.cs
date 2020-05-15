using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DeviceApi.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LuneauPortal
{
    public class Startup
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
            services.AddControllers();
            services.AddSignalR();
            AppSettings appSettings = ConfigureSettings(services);
            services.AddSingleton(provider => {
                RSA rsa = RSA.Create();
                rsa.ImportSubjectPublicKeyInfo(
                    source: Convert.FromBase64String(appSettings.Jwt.PublicKey),
                    bytesRead: out int tolo0
                );
                return new RsaSecurityKey(rsa);
            });
            ConfigureAuthentication(services);
        }

        private AppSettings ConfigureSettings(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            return appSettingsSection.Get<AppSettings>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(c =>
            {
                c.AllowAnyOrigin();
                c.AllowAnyMethod();
                c.AllowAnyHeader();
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<RTCHub>("hubs/rtc");
            });
        }
    }
}
