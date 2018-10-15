using KooBoo.Framework.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KooBoo.Framework.BaseClasses
{
    public class KBViewEngine : RazorViewEngine
    {
        public KBViewEngine()
        {
            AreaViewLocationFormats = new[] {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
 
            AreaMasterLocationFormats = new[] {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
 
            AreaPartialViewLocationFormats = new[] {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
 
            ViewLocationFormats = new[] {
            "~/Views/%1/{1}/{0}.cshtml",
            "~/Views/%1/{1}/{0}.vbhtml",
            "~/Views/%1/Shared/{0}.cshtml",
            "~/Views/%1/Shared/{0}.vbhtml",
            "~/Views/Global/{1}/{0}.cshtml",
            "~/Views/Global/{1}/{0}.vbhtml",
            "~/Views/Global/Shared/{0}.cshtml",
            "~/Views/Global/Shared/{0}.vbhtml"
            };
 
            MasterLocationFormats = new[] {
            "~/Views/%1/{1}/{0}.cshtml",
            "~/Views/%1/{1}/{0}.vbhtml",
            "~/Views/%1/Shared/{0}.cshtml",
            "~/Views/%1/Shared/{0}.vbhtml",
            "~/Views/Global/{1}/{0}.cshtml",
            "~/Views/Global/{1}/{0}.vbhtml",
            "~/Views/Global/Shared/{0}.cshtml",
            "~/Views/Global/Shared/{0}.vbhtml"
            };
 
            PartialViewLocationFormats = new[] {
            "~/Views/%1/{1}/{0}.cshtml",
            "~/Views/%1/{1}/{0}.vbhtml",
            "~/Views/%1/Shared/{0}.cshtml",
            "~/Views/%1/Shared/{0}.vbhtml",
            "~/Views/%1/Shared/BoxPartials/{0}.cshtml",
            "~/Views/%1/Shared/BoxPartials/{0}.vbhtml",
            "~/Views/Global/{1}/{0}.cshtml",
            "~/Views/Global/{1}/{0}.vbhtml",
            "~/Views/Global/Shared/{0}.cshtml",
            "~/Views/Global/Shared/{0}.vbhtml",
            "~/Views/Global/Shared/BoxPartials/{0}.cshtml",
            "~/Views/Global/Shared/BoxPartials/{0}.vbhtml"
            };
        }
 
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            var PassedController = controllerContext.Controller as MultiTenantController;
            
            return base.CreatePartialView(controllerContext, partialPath.Replace("%1", PassedController.CurrentTenant.FolderName));
        }
 
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            var PassedController = controllerContext.Controller as MultiTenantController;
            
            return base.CreateView(controllerContext, viewPath.Replace("%1", PassedController.CurrentTenant.FolderName), masterPath.Replace("%1", PassedController.CurrentTenant.FolderName));
        }
 
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            var PassedController = controllerContext.Controller as MultiTenantController;
            
            return base.FileExists(controllerContext, virtualPath.Replace("%1", PassedController.CurrentTenant.FolderName));
        }
    }
}
