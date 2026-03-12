using System.Threading;
using System.Threading.Tasks;
using Soenneker.Cloudflare.OpenApiClient.Models;

namespace Soenneker.Cloudflare.DnsSettings.Abstract;

/// <summary>
/// Utility for managing Cloudflare DNS settings
/// </summary>
public interface ICloudflareDnsSettingsUtil
{
    /// <summary>
    /// Gets the current DNSSEC status for a zone
    /// </summary>
    /// <param name="zoneId">The Cloudflare zone ID</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>The DNSSEC status</returns>
    ValueTask<bool> GetDnssecStatus(string zoneId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the full DNSSEC details for a zone
    /// </summary>
    /// <param name="zoneId">The Cloudflare zone ID</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>The DNSSEC details including multi-signer, presigned, and NSEC3 settings</returns>
    ValueTask<Dnssec_dnssec?> GetDnssecDetails(string zoneId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Enables DNSSEC for a zone
    /// </summary>
    /// <param name="zoneId">The Cloudflare zone ID</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>True if successful</returns>
    ValueTask<bool> EnableDnssec(string zoneId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disables DNSSEC for a zone
    /// </summary>
    /// <param name="zoneId">The Cloudflare zone ID</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>True if successful</returns>
    ValueTask<bool> DisableDnssec(string zoneId, CancellationToken cancellationToken = default);
}
