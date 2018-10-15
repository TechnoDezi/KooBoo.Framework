using KooBoo.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace KooBoo.Framework.BaseClasses
{
    public class KBApplication : System.Web.HttpApplication
    {
        public static List<AppTenant> Tenants;

        protected void Application_Error(object sender, EventArgs e)
        {
            var httpException = Server.GetLastError() as HttpException;

            if (httpException != null)
            {
                //Handle page not found exception
                if (httpException.GetHttpCode() == 404)
                {
                    Response.Clear();

                    var rd = new RouteData();
                    rd.Values["controller"] = "Home";
                    rd.Values["action"] = "Index";

                    Server.ClearError();

                    Response.Redirect("~/", true);
                }
            }
        }
    }
}
