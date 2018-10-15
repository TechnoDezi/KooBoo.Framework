using KooBoo.Framework.BaseClasses;
using KooBoo.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KooBoo.Framework.BaseClasses
{
    public class MultiTenantController : Controller
    {
        public AppTenant CurrentTenant;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CurrentTenant = GetCurrentTenant(filterContext.HttpContext.Request.Url.Host.ToLower());
            System.Web.HttpContext.Current.Session["CurrentTenant"] = CurrentTenant;
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            
        }

        internal static AppTenant GetCurrentTenant(string host)
        {
            if (host == null)
            {
                host = "";
            }

            var Tenant = KBApplication.Tenants.Where(p => //This Tenants Loaded in memory on Application_start()
            {
                var match = p.Code + "."; //p.FolderName holds "site1" or "site2" etc...
                return host.StartsWith(match); //is it http://site1.com?
            }).FirstOrDefault();

            if (Tenant == null)
            {
                Tenant = KBApplication.Tenants.Where(p =>
                {
                    var match = p.Code + ".";
                    return host.Contains("." + match); //is it http://www.site1.com?
                }).FirstOrDefault();
            }

            if (Tenant == null && host.ToLower().Contains("localhost"))
            {
                Tenant = KBApplication.Tenants.Where(p => p.Code == "localhost").FirstOrDefault();
            }

            return Tenant ?? KBApplication.Tenants[0];
        }
    }
}
