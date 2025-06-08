using Soenneker.Cloudflare.DnsSettings.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Cloudflare.DnsSettings.Tests;

[Collection("Collection")]
public sealed class CloudflareDnsSettingsUtilTests : FixturedUnitTest
{
    private readonly ICloudflareDnsSettingsUtil _util;

    public CloudflareDnsSettingsUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<ICloudflareDnsSettingsUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
