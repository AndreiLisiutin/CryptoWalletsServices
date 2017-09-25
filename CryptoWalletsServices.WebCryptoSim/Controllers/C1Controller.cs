using CryptoWalletsServices.Core.DataInterfaces.Repositories;
using CryptoWalletsServices.Core.Models.Business;
using CryptoWalletsServices.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CryptoWalletsServices.WebCryptoSim.Controllers
{
	[RoutePrefix("api/C1")]
	public class C1Controller : ApiController
	{
		public IC1Repository C1Repository { get; set; }

		[Route("GetCertificates/{msisdn}")]
		[HttpGet]
		public C1Rescponse<List<Guid>> GetCertificates(string msisdn)
		{
			Argument.Require(!string.IsNullOrWhiteSpace(msisdn), "Номер телефона пустой.");
			var response = C1Repository.GetCertificates(msisdn);
			return response;
		}

	}
}
