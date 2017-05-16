using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunriseSunset.Abstractions;

namespace SunriseSunset.Models
{
    public class SunriseSunsetData : ISunriseSunsetData
    {
        public SunriseSunsetData(string address)
        {
            Address = address;
        }

        public string Address { get; }

        public DateTime CurrentTime { get; set; }

        public string TimeZoneName { get; set; }

        public DateTime? Sunrise { get; set; }

        public DateTime? Sunset { get; set; }

        public bool IsDaylight(int plusHours = 0)
        {
            var now = CurrentTime.AddHours(plusHours);

            if (now > Sunrise && now < Sunset)
                return true;

            return false;
        }
    }
}
