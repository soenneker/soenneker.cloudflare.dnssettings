using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Soenneker.Cloudflare.DnsSettings.Abstract;
using Soenneker.Cloudflare.OpenApiClient;
using Soenneker.Cloudflare.OpenApiClient.Models;
using Soenneker.Cloudflare.Utils.Client.Abstract;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Cloudflare.DnsSettings;

/// <inheritdoc cref="ICloudflareDnsSettingsUtil"/>
public sealed class CloudflareDnsSettingsUtil : ICloudflareDnsSettingsUtil
{
    private readonly ICloudflareClientUtil _clientUtil;
    private readonly ILogger<CloudflareDnsSettingsUtil> _logger;

    public CloudflareDnsSettingsUtil(ICloudflareClientUtil clientUtil, ILogger<CloudflareDnsSettingsUtil> logger)
    {
        _clientUtil = clientUtil;
        _logger = logger;
    }

    public async ValueTask<bool> GetDnssecStatus(string zoneId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting DNSSEC status for zone {ZoneId}", zoneId);
        try
        {
            CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
            Dnssec_dnssec_response_single? response = await client.Zones[zoneId].Dnssec.GetAsync(cancellationToken: cancellationToken).NoSync();
            bool isActive = response.Result?.Status == Dnssec_status.Active;
            _logger.LogInformation("DNSSEC status for zone {ZoneId}: {Status}", zoneId, isActive ? "Active" : "Inactive");
            return isActive;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get DNSSEC status for zone {ZoneId}. Error: {ErrorMessage}", zoneId, ex.Message);
            return false;
        }
    }

    public async ValueTask<Dnssec_dnssec?> GetDnssecDetails(string zoneId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting DNSSEC details for zone {ZoneId}", zoneId);
        try
        {
            CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
            Dnssec_dnssec_response_single? response = await client.Zones[zoneId].Dnssec.GetAsync(cancellationToken: cancellationToken).NoSync();
            
            if (response?.Result != null)
            {
                _logger.LogInformation(
                    "Retrieved DNSSEC details for zone {ZoneId}. Status: {Status}, MultiSigner: {MultiSigner}, Presigned: {Presigned}, NSEC3: {Nsec3}",
                    zoneId,
                    response.Result.Status,
                    response.Result.DnssecMultiSigner,
                    response.Result.DnssecPresigned,
                    response.Result.DnssecUseNsec3);
            }
            else
            {
                _logger.LogWarning("No DNSSEC details found for zone {ZoneId}", zoneId);
            }
            
            return response?.Result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get DNSSEC details for zone {ZoneId}. Error: {ErrorMessage}", zoneId, ex.Message);
            return null;
        }
    }

    public async ValueTask<bool> EnableDnssec(string zoneId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Enabling DNSSEC for zone {ZoneId}", zoneId);
        try
        {
            CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
            var requestBody = new Dnssec_edit_dnssec_status_RequestBody_application_json
            {
                Status = Dnssec_edit_dnssec_status_RequestBody_application_json_status.Active
            };
            Dnssec_dnssec_response_single? response = await client.Zones[zoneId].Dnssec.PatchAsync(requestBody, cancellationToken: cancellationToken).NoSync();
            bool success = response.Success ?? false;
            
            if (success)
            {
                _logger.LogInformation("Successfully enabled DNSSEC for zone {ZoneId}", zoneId);
            }
            else
            {
                _logger.LogWarning("Failed to enable DNSSEC for zone {ZoneId}. Response success: {Success}", zoneId, success);
            }
            
            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enable DNSSEC for zone {ZoneId}. Error: {ErrorMessage}", zoneId, ex.Message);
            return false;
        }
    }

    public async ValueTask<bool> DisableDnssec(string zoneId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Disabling DNSSEC for zone {ZoneId}", zoneId);
        try
        {
            CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
            var requestBody = new Dnssec_edit_dnssec_status_RequestBody_application_json
            {
                Status = Dnssec_edit_dnssec_status_RequestBody_application_json_status.Disabled
            };
            Dnssec_dnssec_response_single? response = await client.Zones[zoneId].Dnssec.PatchAsync(requestBody, cancellationToken: cancellationToken).NoSync();
            bool success = response.Success ?? false;
            
            if (success)
            {
                _logger.LogInformation("Successfully disabled DNSSEC for zone {ZoneId}", zoneId);
            }
            else
            {
                _logger.LogWarning("Failed to disable DNSSEC for zone {ZoneId}. Response success: {Success}", zoneId, success);
            }
            
            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to disable DNSSEC for zone {ZoneId}. Error: {ErrorMessage}", zoneId, ex.Message);
            return false;
        }
    }
}