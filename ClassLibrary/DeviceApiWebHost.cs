using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DeviceApi
{
    public class DeviceApiWebHost
    {
        public static Task RunWebHostBuilder(string[] args, string publicKey, ACommunicationProtocol communicationProtocol)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(configureServices =>
                {
                    configureServices.AddSingleton(provider =>
                    {
                        RSA rsa = RSA.Create();
                        rsa.FromXmlString(publicKey);
                        return new RsaSecurityKey(rsa);
                    });
                    configureServices.AddSingleton(communicationProtocol);
                })
                .UseStartup<Startup>()
                .UseUrls("http://10.0.0.6:3001")
                .Build()
                .RunAsync();
        }
    }
}
