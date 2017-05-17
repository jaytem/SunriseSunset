using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunriseSunset.Abstractions;
using SunriseSunset.Models;

namespace SunriseSunset.Test
{
    [TestClass]
    public class SunriseSunsetTest
    {
        string address;
        
        [TestInitialize]
        public void Setup()
        {
            address = "3817 McCoy Dr. Suite 105 Aurora, IL 60504";

            // TODO: set up new USNavyData object with defined values and use for tests


        }
        

        [TestMethod]
        public void SunriseSunset_OnGetCommand_Sunrise_IsPopulated()
        {
            DotNetStarter.ApplicationContext.Startup();
            var locator = DotNetStarter.ApplicationContext.Default.Locator;
            var service = locator.Get<ISunriseSunsetService>();

            // Arrange
            ISunriseSunsetData data;

            // Act
            data = service.Get(address);

            // Assert
            Assert.IsNotNull(data.Sunrise);
        }

        [TestMethod]
        public void SunriseSunset_OnGetCommand_Sunset_IsPopulated()
        {
            DotNetStarter.ApplicationContext.Startup();
            var locator = DotNetStarter.ApplicationContext.Default.Locator;
            var service = locator.Get<ISunriseSunsetService>();

            // Arrange
            ISunriseSunsetData data;

            // Act
            data = service.Get(address);

            // Assert
            Assert.IsNotNull(data.Sunset);
        }

        [TestMethod]
        public void SunsriseSunset_TimezoneName_IsPopulated()
        {
            DotNetStarter.ApplicationContext.Startup();
            var locator = DotNetStarter.ApplicationContext.Default.Locator;
            var service = locator.Get<ISunriseSunsetService>();

            // Arrange
            ISunriseSunsetData data;

            // Act
            data = service.Get(address);

            // Assert
            Assert.IsNotNull(data.TimeZoneName);
            Assert.AreEqual("Central Standard Time", data.TimeZoneName);
        }

        [TestMethod]
        public void SunsriseSunset_TimezoneNameTransform_IsCorrect()
        {
            DotNetStarter.ApplicationContext.Startup();
            var locator = DotNetStarter.ApplicationContext.Default.Locator;
            var service = locator.Get<ISunriseSunsetService>();

            // Arrange
            ISunriseSunsetData data;

            // Act
            data = service.Get("2752 Woodlawn Dr #518, Honolulu, HI 96822");

            // Assert
            Assert.IsNotNull(data.TimeZoneName);
            Assert.AreEqual("Hawaiian Standard Time", data.TimeZoneName);
        }

        [TestMethod]
        public void SunriseSunset_IsDaytime_Check()
        {
            DotNetStarter.ApplicationContext.Startup();
            var locator = DotNetStarter.ApplicationContext.Default.Locator;
            var service = locator.Get<ISunriseSunsetService>();

            // Arrange
            ISunriseSunsetData data;

            // Act
            data = service.Get(address);

            // Assert
            Assert.IsNotNull(data.IsDaylight());
            Assert.AreEqual(true, data.IsDaylight());
        }

        [TestMethod]
        public void SunriseSunset_IsNighttime_Check()
        {
            DotNetStarter.ApplicationContext.Startup();
            var locator = DotNetStarter.ApplicationContext.Default.Locator;
            var service = locator.Get<ISunriseSunsetService>();

            // Arrange
            ISunriseSunsetData data;

            // Act
            data = service.Get(address);

            // Assert
            Assert.IsNotNull(data.IsDaylight());
            Assert.AreEqual(false, data.IsDaylight());
        }
    }
}
