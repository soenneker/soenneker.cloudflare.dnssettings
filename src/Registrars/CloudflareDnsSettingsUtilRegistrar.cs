using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Cloudflare.DnsSettings.Abstract;
using Soenneker.Cloudflare.Utils.Client.Registrars;

namespace Soenneker.Cloudflare.DnsSettings.Registrars;

/// <summary>
/// A utility for managing Cloudflare DNS settings
/// </summary>
public static class CloudflareDnsSettingsUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="ICloudflareDnsSettingsUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddCloudflareDnsSettingsUtilAsSingleton(this IServiceCollection services)
    {
        services.AddCloudflareClientUtilAsSingleton().TryAddSingleton<ICloudflareDnsSettingsUtil, CloudflareDnsSettingsUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="ICloudflareDnsSettingsUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddCloudflareDnsSettingsUtilAsScoped(this IServiceCollection services)
    {
        services.AddCloudflareClientUtilAsSingleton().TryAddScoped<ICloudflareDnsSettingsUtil, CloudflareDnsSettingsUtil>();

        return services;
    }
}
