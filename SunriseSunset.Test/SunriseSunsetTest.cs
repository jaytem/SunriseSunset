using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunriseSunset.Abstractions;
using SunriseSunset.Models;
using DotNetStarter.Abstractions;
using System.Net;

namespace SunriseSunset.Test
{
    [TestClass]
    public class SunriseSunsetTest
    {
        string address;
        SunriseSunsetData invalidAddress;
     
        private ISunriseSunsetData sut;
        private ISunriseSunsetData sutIP;

        Import<ISunriseSunsetService> service;

        [TestInitialize]
        public void Setup()
        {
            address = "3817 McCoy Dr. Suite 105 Aurora, IL 60504";
            invalidAddress = new SunriseSunsetData("123", null);


            IPAddress ipAddress;
            IPAddress.TryParse("207.223.36.84", out ipAddress);


            // Act
            sut = service.Service.GetByAddress(address);
            sutIP = service.Service.GetByIP(ipAddress);
        }


        [TestMethod]
        public void SunriseSunset_OnGetCommand_SunriseByAddress_IsPopulated()
        {
            // Assert
            Assert.IsNotNull(sut.Sunrise);
            Assert.AreEqual(DateTime.Today.DayOfYear, sut.Sunrise.Value.DayOfYear);
        }

        [TestMethod]
        public void SunriseSunset_OnGetCommand_SunriseByIP_IsPopulated()
        {
            // Assert
            Assert.IsNotNull(sutIP.Sunrise);
            Assert.AreEqual(DateTime.Today.DayOfYear, sutIP.Sunrise.Value.DayOfYear);
        }

        [TestMethod]
        public void SunriseSunset_OnGetCommand_SunsetByAddress_IsPopulated()
        {
            // Assert
            Assert.IsNotNull(sut.Sunset);
            Assert.AreEqual(DateTime.Today.DayOfYear, sut.Sunset.Value.DayOfYear);
        }

        [TestMethod]
        public void SunriseSunset_OnGetCommand_SunsetByIP_IsPopulated()
        {
            // Assert
            Assert.IsNotNull(sutIP.Sunset);
            Assert.AreEqual(DateTime.Today.DayOfYear, sutIP.Sunset.Value.DayOfYear);
        }

        [TestMethod]
        public void SunsriseSunset_TimezoneNameByAddress_IsPopulated()
        {
            // Assert
            Assert.IsNotNull(sut.TimeZoneName);
        }

        [TestMethod]
        public void SunsriseSunset_TimezoneNameByIP_IsPopulated()
        {
            // Assert
            Assert.IsNotNull(sutIP.TimeZoneName);
        }

        [TestMethod]
        public void SunsriseSunset_CurrentTimeByAddress_IsPopulated()
        {
            // Assert
            Assert.IsNotNull(sut.CurrentTime());
        }

        [TestMethod]
        public void SunsriseSunset_CurrentTimeByIP_IsPopulated()
        {
            // Assert
            Assert.IsNotNull(sutIP.CurrentTime());
        }

        [TestMethod]
        public void SunriseSunset_Sunrise_IsNull()
        {
            // Assert
            Assert.IsNull(invalidAddress.Sunrise);
        }

        [TestMethod]
        public void SunriseSunset_Sunset_IsNull()
        {
            // Assert
            Assert.IsNull(invalidAddress.Sunset);
        }

        [TestMethod]
        public void SunriseSunset_CurrentTime_GetByAddress_IsMinDate()
        {
            // Assert
           Assert.AreEqual(DateTime.MinValue, invalidAddress.CurrentTime());
        }

        [TestMethod]
        public void SunriseSunset_TimeZoneName_IsNull()
        {
            // Assert
            Assert.IsNull(invalidAddress.TimeZoneName);
        }


    }
}
