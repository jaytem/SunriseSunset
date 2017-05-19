using System.Net;

namespace SunriseSunset.Abstractions
{
    public interface ISunriseSunsetService
    {
        ISunriseSunsetData GetByAddress(string Address);

        ISunriseSunsetData GetByIP(IPAddress IpAddress);
    }
}
