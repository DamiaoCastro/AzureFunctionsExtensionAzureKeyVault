using Microsoft.Azure.WebJobs;
using System;

namespace AzureFunctions.Extensions.KeyVault.Config
{
    public static class KeyVaultJobHostConfigurationExtensions
    {

        /// <summary>
        /// Enables use of the KeyVault extensions for webjobs
        /// </summary>
        /// <param name="config">The <see cref="JobHostConfiguration"/> to configure.</param>
        public static void UseKeyVault(this IWebJobsBuilder config)
        {

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            // Register our extension configuration provider
            config.AddExtension<KeyVaultExtensionConfig>();

        }

    }
}