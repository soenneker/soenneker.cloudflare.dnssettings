using Soenneker.Cloudflare.DnsSettings.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Cloudflare.DnsSettings.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class CloudflareDnsSettingsUtilTests : HostedUnitTest
{
    private readonly ICloudflareDnsSettingsUtil _util;

    public CloudflareDnsSettingsUtilTests(Host host) : base(host)
    {
        _util = Resolve<ICloudflareDnsSettingsUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
