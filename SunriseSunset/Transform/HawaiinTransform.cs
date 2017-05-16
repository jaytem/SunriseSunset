using DotNetStarter.Abstractions;
using SunriseSunset.Abstractions;

namespace SunriseSunset.Transform
{
    [Register(typeof(ITimezoneNameTransform), LifeTime.Singleton, ConstructorType.Greediest, typeof(DaylightTransform))]
    public class HawaiinTransform : ITimezoneNameTransform
    {
        public string Transform(string TimeZoneName)
        {
            if (TimeZoneName == null)
                return string.Empty;

            if (TimeZoneName.Contains("Hawaii-Aleutian"))
                TimeZoneName = TimeZoneName.Replace("Hawaii-Aleutian", "Hawaiian");

            return TimeZoneName;
        }
    }
}
