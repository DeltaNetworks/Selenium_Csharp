using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFram.DataTest
{
    public class TestSettings
    {
        public string Url { get; set; }
        public string Urlus { get; set; }
        public string Urlsso { get; set; }
        public string Headless { get; set; }

        public string BrowserType { get; set; }

        public string DriverManagerWait { get; set; }
        public string DriverManagerInit { get; set; }
        public int ScreenZoom { get; set; }
    }
}
