using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace DemoAspNetCore3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureAppConfiguration((context, config) =>
                {
                    //config.AddUserSecrets<Startup>();

                    //var builtConfig = config.Build();
                    //config.AddAzureKeyVault(
                    //    $"https://{builtConfig["KeyVault:Vault"]}.vault.azure.net/",
                    //    builtConfig["KeyVault:ClientId"],
                    //    builtConfig["KeyVault:ClientSecret"],
                    //    new DefaultKeyVaultSecretManager());
                });
    }
}
