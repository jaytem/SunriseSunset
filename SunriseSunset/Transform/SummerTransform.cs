using DotNetStarter.Abstractions;
using SunriseSunset.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseSunset.Transform
{
    [Register(typeof(ITimezoneNameTransform), LifeTime.Singleton, ConstructorType.Greediest, typeof(DaylightTransform))]
    public class SummerTransform : ITimezoneNameTransform
    {
        public string Transform(string TimeZoneName)
        {
            if (TimeZoneName == null)
                return string.Empty;

            if (TimeZoneName.Contains("Summer"))
                TimeZoneName = TimeZoneName.Replace("Summer", "Summer");

            return TimeZoneName;
        }
    }
}
