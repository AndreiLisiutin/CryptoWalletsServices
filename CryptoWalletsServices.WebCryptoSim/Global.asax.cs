using CryptoWalletsServices.Core.DataInterfaces.Repositories;
using CryptoWalletsServices.Core.DataInterfaces.Utilities;
using CryptoWalletsServices.Core.ServiceInterfaces;
using CryptoWalletsServices.Core.Services;
using CryptoWalletsServices.Data.Repositories;
using CryptoWalletsServices.Data.Utilities;
using LightInject;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;

namespace CryptoWalletsServices.WebCryptoSim
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			var container = new ServiceContainer();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RegisterSercvices(container);
			container.RegisterApiControllers();
			container.EnableWebApi(GlobalConfiguration.Configuration);
		}

		private void RegisterSercvices(ServiceContainer container)
		{
			container.Register<ICryptographyUtility, CryptographyUtility>();
			container.Register<IC1Repository, C1Repository>();
			container.Register<IC1Service, C1Service>();
			container.Register<Configuration>((factory) => WebConfigurationManager.OpenWebConfiguration("~/"));
		}
	}
}
