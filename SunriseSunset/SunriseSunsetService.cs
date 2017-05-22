using DotNetStarter.Abstractions;
using Newtonsoft.Json;
using SunriseSunset.Abstractions;
using SunriseSunset.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace SunriseSunset
{
    [Register(typeof(ISunriseSunsetService))]
    public class SunriseSunsetService : ISunriseSunsetService
    {
        public SunriseSunsetService(IEnumerable<ITimezoneNameTransform> transformers)
        {
            _transformers = transformers;
        }
        
        public ISunriseSunsetData GetByAddress(string Address)
        {
            string latLng = GetLatLongFromAddress(Address);
            var timezone = GetTimeZoneData(latLng);

            var data = new SunriseSunsetData(Address, null);
            data.LatLong = latLng;
            data.TimeZoneName = timezone.timeZoneName;
            data.Sunrise = GetSunriseSunset(true, latLng);
            data.Sunset = GetSunriseSunset(false, latLng);
            data.UtcOffset = timezone.rawOffset;
            data.DstOffset = timezone.dstOffset;

            return data;
        }

        public ISunriseSunsetData GetByIP(IPAddress IpAddress)
        {
            string latLng = GetLatLongFromIP(IpAddress.ToString());
            var timezone = GetTimeZoneData(latLng);

            var data = new SunriseSunsetData(GetCityInfoFromIP(IpAddress.ToString()), IpAddress);
            data.LatLong = latLng;
            data.TimeZoneName = timezone.timeZoneName;
            data.Sunrise = GetSunriseSunset(true, latLng);
            data.Sunset = GetSunriseSunset(false, latLng);
            data.UtcOffset = timezone.rawOffset;
            data.DstOffset = timezone.dstOffset;

            return data;
        }

        #region Private Functions

        /// <summary>
        /// Returns the sunrise or sunset datetime for given latitude and longitude coordates
        /// </summary>
        /// <param name="getSunrise"></param>
        /// <param name="LatLng"></param>
        /// <returns></returns>
        private DateTime? GetSunriseSunset(bool getSunrise, string LatLng)
        {
            USNavySunData sunriseSunsetData = GetSunriseSunsetDataFromNavy(LatLng);

            // Get today's sunrise and sunset from USNavySunData object
            if (sunriseSunsetData != null)
            {
                DateTime? sunrise = null, sunset = null;
                //var timeZone = GetTimeZoneInfo(LatLng);
                var tz = GetTimeZoneData(LatLng);

                foreach (var item in sunriseSunsetData.sundata)
                {
                    if (item.phen == "R")
                        sunrise = DateTime.Parse(item.time).AddSeconds(tz.rawOffset + tz.dstOffset);

                    if (item.phen == "S")
                        sunset = DateTime.Parse(item.time).AddSeconds(tz.rawOffset + tz.dstOffset);
                }

                if (sunrise != null && sunset != null)
                {
                    /* Due to summer tiems and UTC offset, the sunset was coming out after midnight UTC, but was being set to the same date
                     * so sunset was showing as before sunrise. Adding a day to account for this anomoly
                     */
                    if (sunrise > sunset)
                        sunset = sunset.Value.AddDays(1);

                    if (getSunrise)
                        return sunrise;

                    return sunset;
                }
            }

            return null;

        }

        /// <summary>
        /// Get the Geographical data for a given IP Address from freegeoip.net
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        private GeoIPData GetGeoIPData(string IPAddress)
        {
            string cacheKey = FormatCacheKey("LatLngIP", IPAddress);

            GeoIPData geoIP = Cache.Get<GeoIPData>(cacheKey);
            if (geoIP == null)
            {
                string url = string.Format("http://freegeoip.net/json/{0}", IPAddress);
                geoIP = GetAsyncResult<GeoIPData>(url);
            }

            if (geoIP != null)
            {
                Cache.Set(cacheKey, geoIP, 43200);
            }

            return geoIP;
        }

        /// <summary>
        /// Fetch the data from remote web service and deserialize the JSON object into a .Net class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        private T GetAsyncResult<T>(string url)
        {
            string json = string.Empty;

            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            client.Timeout = new TimeSpan(0, 0, 0, 5, 0); /* Set timeout to 5 seconds in case the remote system doesn't respond in a timely manner */

            using (client)
            {
                var result = client.GetAsync(url).ContinueWith((taskwithmsg) =>
                {
                    json = taskwithmsg.Result.Content.ReadAsStringAsync().Result;
                });
                result.Wait();
            }

            return (string.IsNullOrEmpty(json)) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Get the Latitude and Longitude for a given address
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        private string GetLatLongFromAddress(string Address)
        {
            string cacheKey = FormatCacheKey("LatLngAddress", Address);

            LatLongData latLng = Cache.Get<LatLongData>(cacheKey);
            if (latLng == null)
            {
                string url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}", HttpUtility.UrlEncode(Address));
                latLng = GetAsyncResult<LatLongData>(url);
            }

            if (latLng != null && latLng.results != null)
            {
                Cache.Set(cacheKey, latLng, 43200);

                return string.Format("{0},{1}", latLng.results[0].geometry.location.lat, latLng.results[0].geometry.location.lng);
            }

            return null;
        }

        /// <summary>
        /// Get the Latitude and Longitude for a given IP address
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        private string GetLatLongFromIP(string IPAddress)
        {
            var geoIP = GetGeoIPData(IPAddress);

            if (geoIP != null)
                return string.Format("{0},{1}", geoIP.latitude, geoIP.longitude);

            return null;
        }

        /// <summary>
        /// Get a TimeZoneData object for given latitude and longitude corrdinates. With that, you can calculate the current time for the given timezone
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public TimeZoneData GetTimeZoneData(string latLng)
        {
            string cacheKey = FormatCacheKey("TimeZoneData", latLng);

            TimeZoneData timeZone = Cache.Get<TimeZoneData>(cacheKey);

            if (timeZone == null)
            {
                // make sure the lat and long corrdinates are not empty befor continuing on
                if (!string.IsNullOrEmpty(latLng))
                {
                    // Goolge api requires the number of seconds from midnight on January 1, 1970 to get a timezone
                    int seconds = (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;

                    // Google api requires a developer key for this api
                    string googleKey = ConfigurationManager.AppSettings.Get("GoogleKey");
                    
                    string url = string.Format("https://maps.googleapis.com/maps/api/timezone/json?location={0}&timestamp={1}&key={2}", latLng, seconds, googleKey);
                    timeZone = GetAsyncResult<TimeZoneData>(url);

                    if (timeZone != null)
                        Cache.Set(cacheKey, timeZone, 43200);
                         
                }
            }

            return timeZone;
        }

        /// <summary>
        /// Get the Sun data JSON object from the US Navy web service
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        private USNavySunData GetSunriseSunsetDataFromNavy(string latLng)
        {
            // Get the Sunrise/Sunset data from the US Navy web service api
            string cacheKey = FormatCacheKey("USNavySunriseSunset", latLng);
            USNavySunData sunriseSunsetData = Cache.Get<USNavySunData>(cacheKey);

            if (sunriseSunsetData == null)
            {
                var url = string.Format("http://api.usno.navy.mil/rstt/oneday?date={0}&coords={1}", DateTime.Now.ToString("MM/dd/yyyy"), latLng);
                sunriseSunsetData = GetAsyncResult<USNavySunData>(url);

                if (sunriseSunsetData != null)
                    Cache.Set(cacheKey, sunriseSunsetData, 43200);
            }

            return sunriseSunsetData;
        }


        /// <summary>
        /// Returns a string with the general city,state, zipcode, and country code based on a give IP Address
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        private string GetCityInfoFromIP(string IPAddress)
        {
            var geoIP = GetGeoIPData(IPAddress);

            if (geoIP != null)
                return string.Format("{0}, {1}, {2}, {3}", geoIP.city, geoIP.region_code, geoIP.zip_code, geoIP.country_code);

            return null;
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Runs the time zone name through all of the transforms, to account for known differences between retrieved and expected time zone names
        /// </summary>
        /// <param name="TimeZoneName"></param>
        /// <returns></returns>
        private IEnumerable<ITimezoneNameTransform> _transformers;
        private string TransformTimeZoneName(string TimeZoneID)
        {
            string temp = TimeZoneID;

            _transformers.All(x => { temp = x.Transform(temp); return true; });

            return temp;
        }

        /// <summary>
        /// Returns a standardized cache key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private string FormatCacheKey(string key, string latlng)
        {
            return string.Format("Custom:Cache:{0}:LatLong={1}", key, HttpUtility.UrlEncode(latlng));
        }

        #endregion
    }
}
