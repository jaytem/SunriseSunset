﻿using System;

namespace SunriseSunset.Abstractions
{
    public interface ISunriseSunsetData
    {
        string Address { get; }

        string IPAddress { get; }
        
        string LatLong { get; set; }

        string TimeZoneName { get; set; }

        DateTime CurrentTime();

        DateTime? Sunrise { get; set; }

        DateTime? Sunset { get; set; }

        bool IsDaylight(int plusHours = 0);
    }
}
