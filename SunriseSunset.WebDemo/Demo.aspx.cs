using System;
using DotNetStarter.Abstractions;
using SunriseSunset.Abstractions;
using System.Text;
using System.Net;
using SunriseSunset.Models;

namespace SunriseSunset.WebDemo
{
    public partial class Demo : System.Web.UI.Page
    {
        Import<ISunriseSunsetService> _SunriseSunsetService;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void btnGetSunriseSunsetDataByIP_Click(object sender, EventArgs e)
        {
            int plusHours = int.Parse(txtPlusHours.Text.Trim());

            var IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(IP))
                IP = Request.ServerVariables["REMOTE_ADDR"];

            if (IP == "127.0.0.1")
                IP = "185.186.76.101";//"207.223.36.84";

            IPAddress ipAddress;
            IPAddress.TryParse(IP, out ipAddress);


            //Start time for lookup run time
            DateTime startTime = DateTime.Now;

            StringBuilder output = new StringBuilder();

            output.Append(GetSunriseSunsetData(null, ipAddress));


            // End time for lookup run time
            DateTime endTime = DateTime.Now;
            TimeSpan processTime = endTime - startTime;

            litoutput.Text += string.Format("<br />Total Run Time: {0} milliseconds<br /><br />{1}<br /><hr />", processTime.Milliseconds, output.ToString());
        }

        protected void btnGetnSunriseSunsetData_Click(object sender, EventArgs e)
        {
            string[] addressList = new string[]
           {
                "Champ de Mars, 5 Avenue Anatole France, 75007 Paris, France",
                "Westminster, London SW1A 0AA, UK",
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

            foreach (var addr in addressList)
            {
                output.Append(GetSunriseSunsetData(addr, null));
            }

            // End time for lookup run time
            DateTime endTime = DateTime.Now;
            TimeSpan processTime = endTime - startTime;

            litoutput.Text += string.Format("<br />Total Run Time: {0} milliseconds<br /><br />{1}<br /><hr />", processTime.Milliseconds, output.ToString());

        }


        private string GetSunriseSunsetData(string addr, IPAddress ipAddress)
        {
            int plusHours = int.Parse(txtPlusHours.Text.Trim());
            ISunriseSunsetData data = new SunriseSunsetData(null, null);

            if (!string.IsNullOrEmpty(addr))
                data = _SunriseSunsetService.Service.GetByAddress(addr);
            else if (ipAddress != null)
                data = _SunriseSunsetService.Service.GetByIP(ipAddress);



            if (data.Address != null)
            {
                return string.Format("Address: {0}<br />IP Address: {7}<br />Lat/Long: {8}<br />TimeZone: {1}<br />Sunrise: {2}<br />Sunset: {3}<br />Current local time: {5}<br />Test local time: {6}<br />IsDay: {4}<br /><br />",
                    data.Address,
                    data.TimeZoneName,
                    data.Sunrise,
                    data.Sunset,
                    data.IsDaylight(plusHours),
                    data.CurrentTime(),
                    data.CurrentTime().AddHours(plusHours),
                    data.IPAddress,
                    data.LatLong);
            }

            return null;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            litoutput.Text = string.Empty;
        }


    }
}