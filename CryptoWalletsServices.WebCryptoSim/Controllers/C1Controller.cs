using CryptoWalletsServices.Core.DataInterfaces.Repositories;
using CryptoWalletsServices.Core.Models.Business;
using CryptoWalletsServices.Core.ServiceInterfaces;
using CryptoWalletsServices.Utils;
using CryptoWalletsServices.WebCryptoSim.Models;
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
		public IC1Service c1Service { get; set; }

		public C1Controller(IC1Service c1Service)
		{
			this.c1Service = c1Service;
		}

		[Route("GetCertificates")]
		[HttpPost]
		public C1Rescponse<List<Guid>> GetCertificates([FromBody] GetCertificatesViewModel model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			Argument.Require(!string.IsNullOrWhiteSpace(model.Msisdn), "Номер телефона пустой.");
			var response = c1Service.GetCertificates(model.Msisdn);
			return response;
		}

		[Route("GenerateCertificate")]
		[HttpPost]
		public C1Rescponse<Guid> GenerateCertificate([FromBody] GenerateCertificateRequest model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			var response = c1Service.GenerateCertificate(model);
			return response;
		}

		[Route("Activate")]
		[HttpPost]
		public C1Rescponse<bool> Activate([FromBody] ActivateViewModel model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			var response = c1Service.Activate(model.Msisdn, model.Iccid);
			return response;
		}

		[Route("Authenticate")]
		[HttpPost]
		public C1Rescponse<string> Authenticate([FromBody] AuthenticateViewModel model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			var response = c1Service.Authenticate(model.CertificateId, model.TextForUser);
			return response;
		}

		[Route("Sign")]
		[HttpPost]
		public C1Rescponse<string> Sign([FromBody] SignViewModel model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			FinanceDocument document = new FinanceDocument(model.DocumentBase64Data, model.DocumentName, model.DocumentMimeType);

			var response = c1Service.Sign(document, model.CertificateId, model.TextForUser);
			return response;
		}

		[Route("GetCertificate")]
		[HttpPost]
		public C1Rescponse<string> GetCertificate([FromBody] GetCertificateViewModel model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			var response = c1Service.GetCertificate(model.CertificateId);
			return response;
		}


		#region получение результатов транзакций

		[Route("GetTransactionInfo")]
		[HttpPost]
		public C1Rescponse<object> GetTransactionInfo([FromBody] GetTransactionInfoViewModel model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			var response = c1Service.GetTransactionInfo<object>(model.TransactionId);
			return response;
		}

		#endregion получение результатов транзакций
	}
}
