using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunriseSunset.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseSunset.Test
{
    [TestClass]
    public class _globalSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            DotNetStarter.ApplicationContext.Startup();
        }
    }
}
