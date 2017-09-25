using CryptoWalletsServices.Core.DataInterfaces.Utilities;
using CryptoWalletsServices.Data.Utilities;
using LightInject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Routing;

namespace CryptoWalletsServices.WebCryptoSim
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			var container = new ServiceContainer();
			GlobalConfiguration.Configure(WebApiConfig.Register);

			container.RegisterApiControllers();

			container.EnableWebApi(GlobalConfiguration.Configuration);
		}

		private void RegisterSercvices(ServiceContainer container)
		{
			container.Register<ICryptographyUtility, CryptographyUtility>();
			container.Register<Configuration>((factory) => WebConfigurationManager.OpenWebConfiguration(null));
		}
	}
}
