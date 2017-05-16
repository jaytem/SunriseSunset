using System;

namespace SunriseSunset.WebDemo
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            DotNetStarter.ApplicationContext.Startup();
        }
    }
}