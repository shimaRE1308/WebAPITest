using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

using System.Diagnostics;
using System.IO;

namespace WebAPITest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);


            Debug.WriteLine("hogehogehoge");

            Directory.CreateDirectory("C:\\aaaa");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Debug.WriteLine("あいうえお");
        }

    }
}
