﻿using SunriseSunset.Abstractions;
using System;

namespace SunriseSunset.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            DotNetStarter.ApplicationContext.Startup();
            var locator = DotNetStarter.ApplicationContext.Default.Locator;
            var service = locator.Get<ISunriseSunsetService>();

            var IP = "207.223.36.84";
            var ipData = service.Get(IP);

            Console.WriteLine(string.Format("By IP Address{1}Address: {0}{1}IP Address:{8}{1}TimeZone: {5}{1}Lat/Long: {7}{1}Sunrise: {2}{1}Sunset: {3}{1}Current local time: {6}{1}IsDay: {4}{1}{1}",
                    ipData.Address,
                    Environment.NewLine,
                    ipData.Sunrise,
                    ipData.Sunset,
                    ipData.IsDaylight(0).ToString(),
                    ipData.TimeZoneName,
                    ipData.CurrentTime,
                    ipData.LatLong,
                    ipData.IPAddress));


            Console.WriteLine("By Address");

            string[] Addresses = new string[]
            {
                "Walt Disney World Resort, Orlando, FL 32830",
                "3817 McCoy Dr. Suite 105 Aurora, IL 60504",
                "Yellowstone National Park, 1 Grand Loop Rd, Yellowstone National Park, WY 82190",
                "1313 Disneyland Dr, Anaheim, CA 92802",
                "432 S Franklin St, Juneau, AK 99801",
                "2752 Woodlawn Dr #518, Honolulu, HI 96822"
            };

            foreach (var addr in Addresses)
            {
                var data = service.Get(addr);

                Console.WriteLine(string.Format("Address: {0}{1}TimeZone: {5}{1}Lat/Long: {7}{1}Sunrise: {2}{1}Sunset: {3}{1}Current local time: {6}{1}IsDay: {4}{1}{1}",
                    data.Address,
                    Environment.NewLine,
                    data.Sunrise,
                    data.Sunset,
                    data.IsDaylight(0).ToString(),
                    data.TimeZoneName,
                    data.CurrentTime,
                    data.LatLong));
                
            }

           

            Console.ReadLine();
        }
    }
}
