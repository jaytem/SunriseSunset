﻿using System;
using SunriseSunset.Abstractions;
using System.Net;

namespace SunriseSunset.Models
{
    public class SunriseSunsetData : ISunriseSunsetData
    {
        public SunriseSunsetData(string address, IPAddress ipAddress)
        {
            Address = address;

            if (ipAddress != null)
                IPAddress = ipAddress.ToString();
        }
           
        public string Address { get; }

        public string IPAddress { get; }

        public string LatLong { get; set; }

        public DateTime CurrentTime()
        {
            var currentTime = DateTime.MinValue;
            
            if (!string.IsNullOrEmpty(TimeZoneName))
                currentTime = DateTime.UtcNow.AddSeconds(UtcOffset + DstOffset);

            return currentTime;
        }

        public string TimeZoneName { get; set; }

        public DateTime? Sunrise { get; set; }

        public DateTime? Sunset { get; set; }

        public bool IsDaylight(int plusHours = 0)
        {
            var currentTime = CurrentTime();

            var now = currentTime.AddHours(plusHours);

            if (now > Sunrise && now < Sunset)
                return true;

            return false;
        }

        public int UtcOffset { get; set; }
        public int DstOffset { get; set; }
    }
}
