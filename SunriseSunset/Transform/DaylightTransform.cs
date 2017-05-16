using DotNetStarter.Abstractions;
using SunriseSunset.Abstractions;

namespace SunriseSunset.Transform
{
    [Register(typeof(ITimezoneNameTransform), LifeTime.Singleton)]
    public class DaylightTransform : ITimezoneNameTransform
    {
        public string Transform(string TimeZoneName)
        {
            if (TimeZoneName == null)
                return string.Empty;

            if (TimeZoneName.Contains("Daylight"))
                TimeZoneName = TimeZoneName.Replace("Daylight", "Standard");

            return TimeZoneName;
        }
    }
}
