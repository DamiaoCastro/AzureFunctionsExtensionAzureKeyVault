using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctions.Extensions.KeyVault.Services
{
    internal class KeyVaultService
    {

        private const int keyVaultClientCacheMinutes = 10;

        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly ObjectCacheService<string> valueCacheService;
        private readonly ObjectCacheService<KeyVaultClient> keyVaultClientCacheService;

        public KeyVaultService()
        {
            valueCacheService = new ObjectCacheService<string>();
            keyVaultClientCacheService = new ObjectCacheService<KeyVaultClient>();
        }

        internal async Task<string> GetValueAsync(KeyVaultAttribute keyVaultAttribute, CancellationToken cancellationToken)
        {
            var key = $"{keyVaultAttribute.ConfigurationNodeName}-{keyVaultAttribute.SecretName}";
            string value = null;

            Exception ex1 = null;

            await semaphoreSlim.WaitAsync();
            try
            {
                value = valueCacheService.GetObject(key, keyVaultAttribute.CacheMinutes);
                if (value == null)
                {

                    (string baseUrl, string clientId, string clientSecret) = KeyVaultAttributeSettingsService.GetSettings(keyVaultAttribute);

                    KeyVaultClient keyVaultClient = keyVaultClientCacheService.GetObject(keyVaultAttribute.ConfigurationNodeName, keyVaultClientCacheMinutes);
                    if (keyVaultClient == null)
                    {
                        KeyVaultCredential credentials = new KeyVaultCredential(
                            new KeyVaultClient.AuthenticationCallback((authority, resource, scope) => GetAccessToken(clientId, clientSecret, authority, resource, scope))
                            );
                        keyVaultClient = new KeyVaultClient(credentials);
                        keyVaultClientCacheService.SetObject(keyVaultAttribute.ConfigurationNodeName, keyVaultClient);
                    }

                    var result = await keyVaultClient.GetSecretAsync(baseUrl, keyVaultAttribute.SecretName, cancellationToken);

                    var resultValue = result.Value;
                    valueCacheService.SetObject(key, resultValue);
                    return resultValue;
                }
            }
            catch (Exception ex)
            {
                ex1 = ex;
            }
            finally
            {
                semaphoreSlim.Release();
            }

            if (ex1 != null) { throw ex1; }

            return value;
        }

        private static async Task<string> GetAccessToken(string clientId, string clientSecret, string authority, string resource, string scope)
        {
            ClientCredential clientCredential = new ClientCredential(clientId, clientSecret);

            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, clientCredential);

            return result.AccessToken;
        }

    }
}