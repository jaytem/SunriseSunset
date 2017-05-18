using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunriseSunset.Abstractions;
using SunriseSunset.Models;
using DotNetStarter.Abstractions;

namespace SunriseSunset.Test
{
    [TestClass]
    public class SunriseSunsetTest
    {
        string address;

        private ISunriseSunsetData sut;

        Import<ISunriseSunsetService> service;

        [TestInitialize]
        public void Setup()
        {
            var address = "3817 McCoy Dr. Suite 105 Aurora, IL 60504";

            // Act
            sut = service.Service.Get(address);
        }


        [TestMethod]
        public void SunriseSunset_OnGetCommand_Sunrise_IsPopulated()
        {
            // Assert
            Assert.IsNotNull(sut.Sunrise);
            Assert.AreEqual(DateTime.Today.DayOfYear, sut.Sunrise.Value.DayOfYear);
        }

        [TestMethod]
        public void SunriseSunset_OnGetCommand_Sunset_IsPopulated()
        {
            // Assert
            Assert.IsNotNull(sut.Sunset);
            Assert.AreEqual(DateTime.Today.DayOfYear, sut.Sunset.Value.DayOfYear);
        }

        [TestMethod]
        public void SunsriseSunset_TimezoneName_IsPopulated()
        {
            // Assert
            Assert.IsNotNull(sut.TimeZoneName);
            Assert.AreEqual("Central Standard Time", sut.TimeZoneName);
        }

        [TestMethod]
        public void SunsriseSunset_TimezoneNameTransform_IsCorrect()
        {
            // Act
            var data = service.Service.Get("2752 Woodlawn Dr #518, Honolulu, HI 96822");

            // Assert
            Assert.IsNotNull(data.TimeZoneName);
            Assert.AreEqual("Hawaiian Standard Time", data.TimeZoneName);
        }

        [TestMethod]
        public void SunriseSunset_IsDaytime_Check()
        {
            SunriseSunsetData data = new SunriseSunsetData(null);
            data.Sunrise = new DateTime(2017, 01, 01, 06, 00, 00);
            data.Sunset = data.Sunrise.Value.AddHours(12);
            data.CurrentTime = data.Sunrise.Value.AddHours(6);

            // Assert
            Assert.AreEqual(true, data.IsDaylight());
        }

        [TestMethod]
        public void SunriseSunset_IsNighttime_Check()
        {
            SunriseSunsetData data = new SunriseSunsetData(null);
            data.Sunrise = new DateTime(2017, 01, 01, 06, 00, 00);
            data.Sunset = data.Sunrise.Value.AddHours(12);
            data.CurrentTime = data.Sunrise.Value.AddHours(15);

            // Assert
            Assert.AreEqual(false, data.IsDaylight());
        }


        [TestMethod]
        //[ExpectedException(typeof(NullReferenceException))]
        public void SunriseSunset_Sunrise_IsNull()
        {
            SunriseSunsetData data = new SunriseSunsetData("aaa");

            // Assert
            Assert.IsNull(data.Sunset);
        }
    }
}
