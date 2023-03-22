namespace AspNetCoreVaultIntegration
{
    public static class VaultExtensions
    {
        public static IConfigurationBuilder AddVault(this IConfigurationBuilder configuration,
        Action<VaultOptions> options)
        {
            var vaultOptions = new VaultConfigurationSource(options);
            configuration.Add(vaultOptions);
            return configuration;
        }
    }

}
