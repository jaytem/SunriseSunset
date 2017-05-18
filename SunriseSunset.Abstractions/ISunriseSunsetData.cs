using System;

namespace SunriseSunset.Abstractions
{
    public interface ISunriseSunsetData
    {
        string Address { get; set; }

        string IPAddress { get; set; }

        string TimeZoneName { get; set; }

        DateTime? CurrentTime { get; set; }

        DateTime? Sunrise { get; set; }

        DateTime? Sunset { get; set; }

        bool IsDaylight(int plusHours = 0);
    }
}
