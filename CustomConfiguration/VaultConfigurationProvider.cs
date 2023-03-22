using Newtonsoft.Json.Linq;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace AspNetCoreVaultIntegration.CustomConfiguration
{
    public class VaultConfigurationProvider : ConfigurationProvider
    {
        private readonly VaultOptions _vaultOptions;
        private readonly IVaultClient _vaultClient;

        public VaultConfigurationProvider(VaultOptions vaultOptions)
        {
            _vaultOptions = vaultOptions;
            var vaultClientSettings = new VaultClientSettings(_vaultOptions.Address,
                new TokenAuthMethodInfo(vaultOptions.Token));
            _vaultClient = new VaultClient(vaultClientSettings);
        }
        public override void Load()
        {
            LoadAsync().GetAwaiter().GetResult();
        }

        public async Task LoadAsync()
        {
            var data = await ReadSettings(_vaultClient, _vaultOptions);
            var dict = new JsonParser().Parse(JObject.FromObject(data));
            foreach (var j in dict)
            {
                Data.Add(j);
            }
        }
        private async Task<IDictionary<string, object>> ReadSettings(IVaultClient client, VaultOptions options)
        {
            var data = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync(path: options.SecretPath, mountPoint: options.MountPath);
            return data.Data.Data;
        }
    }

}
