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
using System.Threading;
using System.Web.Http;

namespace CryptoWalletsServices.WebCryptoSim.Controllers
{
	/// <summary>
	/// Контроллер 1С.
	/// </summary>
	[RoutePrefix("api/FakeC1")]
	public class FakeC1Controller : ApiController
	{
		private const int STATUS_OK = 0;
		private const int STATUS_IN_QUEUE = 101;
		private const int STATUS_BAD_MIN = 400;

		/// <summary>
		/// Сервис 1С.
		/// </summary>
		private IC1Service c1Service { get; set; }

		/// <summary>
		/// Контроллер 1С.
		/// </summary>
		/// <param name="c1Service">Сервис 1С.</param>
		public FakeC1Controller(IC1Service c1Service)
		{
			this.c1Service = c1Service;
		}

		/// <summary>
		/// Метод используется для получения списка идентификаторов сертификатов, доступных Сервис-провайдеру.
		/// </summary>
		/// <param name="model">Параметры.</param>
		/// <returns>Информация о транзакции.</returns>
		[Route("GetCertificates")]
		[HttpGet]
		public C1Rescponse<List<Guid>> GetCertificates([FromUri] GetCertificatesViewModel model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			Argument.Require(!string.IsNullOrWhiteSpace(model.Msisdn), "Номер телефона пустой.");
			C1Rescponse response = c1Service.GetCertificates(model.Msisdn);

			if (response.StatusCode >= STATUS_BAD_MIN)
			{
				return new C1Rescponse<List<Guid>>(response);
			}

			return WaitTillResponse<List<Guid>>(response.TransactionGuid);
		}

		private C1Rescponse<T> WaitTillResponse<T>(Guid transactionId)
		{
			while (true)
			{
				var transactionResponse = c1Service.GetTransactionInfo<T>(transactionId);
				if (transactionResponse.StatusCode == STATUS_OK)
				{
					return transactionResponse;
				}
				if (transactionResponse.StatusCode >= STATUS_BAD_MIN)
				{
					return new C1Rescponse<T>(transactionResponse);
				}
				Thread.Sleep(3000);
			}
		}

		/// <summary>
		/// Метод используется для генерации ключевой пары и выпуска сертификата посредством Удостоверяющего центра.
		/// </summary>
		/// <param name="model">Параметры.</param>
		/// <returns>Информация о транзакции.</returns>
		[Route("GenerateCertificate")]
		[HttpPost]
		public C1Rescponse<Guid> GenerateCertificate([FromBody] GenerateCertificateRequest model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			var response = c1Service.GenerateCertificate(model);

			if (response.StatusCode >= STATUS_BAD_MIN)
			{
				return new C1Rescponse<Guid>(response);
			}

			return WaitTillResponse<Guid>(response.TransactionGuid);
		}

		/// <summary>
		/// Вызов метода производится для активации профиля пользователя для работы с конкретным Сервис-провайдером.
		/// Метод вызывается Сервис-провайдером однократно, но обязательно до других операций, относящихся к данному пользователю.
		/// При активации профиля пользователя производится проверка реквизитов sim-карты и их соответствия с абонентским номером MSISDN.
		/// </summary>
		/// <param name="model">Параметры.</param>
		/// <returns>Информация о транзакции.</returns>
		[Route("Activate")]
		[HttpPost]
		public C1Rescponse<bool> Activate([FromBody] ActivateViewModel model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			var response = c1Service.Activate(model.Msisdn, model.Iccid);

			if (response.StatusCode >= STATUS_BAD_MIN)
			{
				return new C1Rescponse<bool>(response);
			}

			return WaitTillResponse<bool>(response.TransactionGuid);
		}

		/// <summary>
		/// Метод предназначен для аутентификации пользователя с использованием сертификата, к которому у Сервис-провайдера имеется доступ.
		/// </summary>
		/// <param name="model">Параметры.</param>
		/// <returns>Информация о транзакции.</returns>
		[Route("Authenticate")]
		[HttpPost]
		public C1Rescponse<string> Authenticate([FromBody] AuthenticateViewModel model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			var response = c1Service.Authenticate(model.CertificateId, model.TextForUser);

			if (response.StatusCode >= STATUS_BAD_MIN)
			{
				return new C1Rescponse<string>(response);
			}

			return WaitTillResponse<string>(response.TransactionGuid);
		}

		/// <summary>
		/// Метод используется для подписи данных. 
		/// Подписываемые данные передаются в Платформу 1С-SIM, где вычисляется хеш и отправляется на sim-карту пользователя для формирования подписи.
		/// Результатом работы метода является отсоединенная подпись в формате pkcs#7.
		/// </summary>
		/// <param name="model">Параметры.</param>
		/// <returns>Информация о транзакции.</returns>
		[Route("Sign")]
		[HttpPost]
		public C1Rescponse Sign([FromBody] SignViewModel model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			FinanceDocument document = new FinanceDocument(model.DocumentBase64Data, model.DocumentName, model.DocumentMimeType);

			var response = c1Service.Sign(document, model.CertificateId, model.TextForUser);

			if (response.StatusCode >= STATUS_BAD_MIN)
			{
				return new C1Rescponse<string>(response);
			}

			return WaitTillResponse<string>(response.TransactionGuid);
		}

		/// <summary>
		/// Метод используется для получения тела сертификата, к которому Сервис-провайдер имеет доступ.
		/// </summary>
		/// <param name="model">Параметры.</param>
		/// <returns>Информация о транзакции.</returns>
		[Route("GetCertificate")]
		[HttpGet]
		public C1Rescponse<string> GetCertificate([FromUri] GetCertificateViewModel model)
		{
			Argument.Require(model != null, "Параметры не заданы.");
			var response = c1Service.GetCertificate(model.CertificateId);

			if (response.StatusCode >= STATUS_BAD_MIN)
			{
				return new C1Rescponse<string>(response);
			}

			return WaitTillResponse<string>(response.TransactionGuid);
		}

		#region получение результатов транзакций

		/// <summary>
		/// Метод используется для получения состояния транзакции при использовании PULL-режима.
		/// Если транзакция была успешно обработана и предполагается, что метод должен вернуть данные - они будут представлены в поле data.
		/// </summary>
		/// <param name="model">Параметры.</param>
		/// <returns>Информация о транзакции.</returns>
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
