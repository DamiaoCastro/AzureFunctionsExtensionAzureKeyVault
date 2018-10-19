using Microsoft.Azure.WebJobs.Description;
using System;

namespace AzureFunctions.Extensions.KeyVault
{

    [Binding]
    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Parameter)]
    public class KeyVaultAttribute : Attribute
    {

        public KeyVaultAttribute(string configurationNodeName, string secretName)
        {
            ConfigurationNodeName = configurationNodeName;
            SecretName = secretName;
        }

        public string ConfigurationNodeName { get; }

        public string SecretName { get; }

        public int CacheMinutes { get; set; } = 5;

    }
}