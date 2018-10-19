using System;

namespace AzureFunctions.Extensions.KeyVault.Services
{
    internal class KeyVaultAttributeSettingsService
    {
        public const string BaseUrlSettingKey = "BaseUrl";
        public const string ClientIdSettingKey = "ClientId";
        public const string ClientSecretSettingKey = "ClientSecret";

        public static (string baseUrl, string clientId, string clientSecret) GetSettings(KeyVaultAttribute keyVaultAttribute)
        {
            string baseUrl = Environment.GetEnvironmentVariable($"{keyVaultAttribute.ConfigurationNodeName}.{BaseUrlSettingKey}", EnvironmentVariableTarget.Process);
            string clientId = Environment.GetEnvironmentVariable($"{keyVaultAttribute.ConfigurationNodeName}.{ClientIdSettingKey}", EnvironmentVariableTarget.Process);
            string clientSecret = Environment.GetEnvironmentVariable($"{keyVaultAttribute.ConfigurationNodeName}.{ClientSecretSettingKey}", EnvironmentVariableTarget.Process);

            return (baseUrl, clientId, clientSecret);
        }

    }
}
