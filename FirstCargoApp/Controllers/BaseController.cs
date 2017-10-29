using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirstCargoApp.Helper;
using System.Threading;
using System.Globalization;

namespace FirstCargoApp.Controllers
{
    public class BaseController : Controller
    {

        //protected override void ExecuteCore()
        //{
        //    int culture = 0;
        //    if (this.Session == null || this.Session["CurrentCulture"] == null)
        //    {

        //        int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Culture"], out culture);
        //        this.Session["CurrentCulture"] = culture;
        //    }
        //    else
        //    {
        //        culture = (int)this.Session["CurrentCulture"];
        //    }
        //    // calling CultureHelper class properties for setting  
        //    LocalisationHelper.CurrentCulture = culture;

        //    base.ExecuteCore();
        //}

        //initilizing culture on controller initialization
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session["CurrentCulture"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["CurrentCulture"].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["CurrentCulture"].ToString());
            }
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        } 
	}
}