using AzureFunctions.Extensions.KeyVault.Config;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(KeyVaultWebJobStartup), "KeyVault")]
namespace AzureFunctions.Extensions.KeyVault.Config
{
    public class KeyVaultWebJobStartup : IWebJobsStartup
    {
        void IWebJobsStartup.Configure(IWebJobsBuilder builder)
        {
            builder.UseKeyVault();
        }
    }
}
