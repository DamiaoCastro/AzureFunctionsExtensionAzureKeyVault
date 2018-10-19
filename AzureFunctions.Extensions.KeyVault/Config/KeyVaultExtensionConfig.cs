using AzureFunctions.Extensions.KeyVault.Services;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctions.Extensions.KeyVault.Config
{
    [Extension("KeyVault")]
    public partial class KeyVaultExtensionConfig : IExtensionConfigProvider
    {
        void IExtensionConfigProvider.Initialize(ExtensionConfigContext context)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }


            var keyVaultService = new KeyVaultService();

            Func<KeyVaultAttribute, CancellationToken, Task<string>> builder =
                (KeyVaultAttribute keyVaultAttribute, CancellationToken cancellationToken) =>
                {
                    return keyVaultService.GetValueAsync(keyVaultAttribute, cancellationToken);
                };

            context
                .AddBindingRule<KeyVaultAttribute>()
                .BindToInput(builder)
                ;

        }

    }
}
