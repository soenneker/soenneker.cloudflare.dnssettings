[![](https://img.shields.io/nuget/v/soenneker.cloudflare.dnssettings.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.dnssettings/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cloudflare.dnssettings/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.cloudflare.dnssettings/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.cloudflare.dnssettings.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.dnssettings/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cloudflare.dnssettings/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.cloudflare.dnssettings/actions/workflows/codeql.yml)

# Soenneker.Cloudflare.DnsSettings

Reads, enables, and disables DNSSEC for Cloudflare zones.

## Installation

```bash
dotnet add package Soenneker.Cloudflare.DnsSettings
```

## Configuration

```json
{
  "Cloudflare": {
    "ApiKey": "your-api-token"
  }
}
```

Use a scoped token with permission to read and edit DNS settings for the target zone.

## Registration and usage

```csharp
using Soenneker.Cloudflare.DnsSettings.Abstract;
using Soenneker.Cloudflare.DnsSettings.Registrars;

services.AddCloudflareDnsSettingsUtilAsScoped();

public sealed class DnssecService(ICloudflareDnsSettingsUtil dnsSettings)
{
    public ValueTask<bool> IsEnabled(string zoneId, CancellationToken cancellationToken)
    {
        return dnsSettings.GetDnssecStatus(zoneId, cancellationToken);
    }

    public ValueTask<bool> Enable(string zoneId, CancellationToken cancellationToken)
    {
        return dnsSettings.EnableDnssec(zoneId, cancellationToken);
    }
}
```

`GetDnssecDetails` returns Cloudflare's generated DNSSEC model when DS material and status details are needed. Singleton registration is also available.

## Operational behavior

The boolean methods return `false` both when DNSSEC is inactive or Cloudflare rejects/fails the request; failures are logged. `GetDnssecDetails` returns `null` on failure or an empty response. Cancellation is propagated to the caller.

Disabling DNSSEC while a DS record remains published at the parent zone can make the domain fail DNSSEC validation. Coordinate changes with the domain registrar and verify the delegation state before and after disabling it.
