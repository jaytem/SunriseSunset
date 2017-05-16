/* Generated from U.S. Navy web service JSON http://aa.usno.navy.mil/data/docs/api.php */

namespace SunriseSunset.Models
{
    public class USNavySunData
    {
        public Sundata[] sundata { get; set; }
    }
    public class Sundata
    {
        public string phen { get; set; }
        public string time { get; set; }
    }
}
