using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplication
{
	public class Global : HttpApplication
	{
		#region Methods

		protected void Application_Start(object sender, EventArgs e)
		{
			AreaRegistration.RegisterAllAreas();

			RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			RouteTable.Routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
				);
		}

		#endregion
	}
}