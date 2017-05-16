using System;
using DotNetStarter.Abstractions;
using SunriseSunset.Abstractions;
using System.Text;

namespace SunriseSunset.WebDemo
{
    public partial class Demo : System.Web.UI.Page
    {
        Import<ISunriseSunsetService> _SunriseSunsetService;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void btnGetnSunriseSunsetData_Click(object sender, EventArgs e)
        {
            int plusHours = int.Parse(txtPlusHours.Text.Trim());

            string[] addressList = new string[]
            {
                "Walt Disney World Resort, Orlando, FL 32830",
                "3817 McCoy Dr. Suite 105 Aurora, IL 60504",
                "Yellowstone National Park, 1 Grand Loop Rd, Yellowstone National Park, WY 82190",
                "1313 Disneyland Dr, Anaheim, CA 92802",
                "432 S Franklin St, Juneau, AK 99801",
                "2752 Woodlawn Dr #518, Honolulu, HI 96822"
            };

            //Start time for lookup run time
            DateTime startTime = DateTime.Now;

            StringBuilder output = new StringBuilder();

            foreach(var addr in addressList)
            {
                var data = _SunriseSunsetService.Service.Get(addr);

                output.Append(string.Format("Address: {0}<br />TimeZone: {1}<br />Sunrise: {2}<br />Sunset: {3}<br />IsDay: {4}<br /><br />",
                    data.Address,
                    data.TimeZoneName, 
                    data.Sunrise,
                    data.Sunset,
                    data.IsDaylight(plusHours)));
            }

            // End time for lookup run time
            DateTime endTime = DateTime.Now;
            TimeSpan processTime = endTime - startTime;

            litoutput.Text += string.Format("<br />Run Time: {0} milliseconds<br /><br />{1}<br /><hr />", processTime.Milliseconds, output.ToString());
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            litoutput.Text = string.Empty;
        }
    }
}