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
        public SunriseSunsetData()
        { }
           

        public string Address { get; set; }

        public string IPAddress { get; set; }

        public DateTime? CurrentTime { get; set; }

        public string TimeZoneName { get; set; }

        public DateTime? Sunrise { get; set; }

        public DateTime? Sunset { get; set; }

        public bool IsDaylight(int plusHours = 0)
        {
            var now = CurrentTime?.AddHours(plusHours);

            if (now > Sunrise && now < Sunset)
                return true;

            return false;
        }
    }
}
