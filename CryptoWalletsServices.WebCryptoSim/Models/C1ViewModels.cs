using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoWalletsServices.WebCryptoSim.Models
{
	/// <summary>
	/// Параметры получения списка сертификатов.
	/// </summary>
	public class GetCertificatesViewModel
	{
		/// <summary>
		/// Номер телефона.
		/// </summary>
		public string Msisdn { get; set; }
	}

	/// <summary>
	/// Параметры активации пользователя.
	/// </summary>
	public class ActivateViewModel
	{
		/// <summary>
		/// Номер телефона.
		/// </summary>
		public string Msisdn { get; set; }

		/// <summary>
		/// Идентификатор симки.
		/// </summary>
		public string Iccid { get; set; }
	}

	/// <summary>
	/// Параметры аутентификации.
	/// </summary>
	public class AuthenticateViewModel
	{
		/// <summary>
		/// Идентификатор сертификата.
		/// </summary>
		public Guid CertificateId { get; set; }

		/// <summary>
		/// Текст для пользователя.
		/// </summary>
		public string TextForUser { get; set; }
	}

	/// <summary>
	/// Параметры подписи документа.
	/// </summary>
	public class SignViewModel
	{
		/// <summary>
		/// Документ в base64.
		/// </summary>
		public string DocumentBase64Data { get; set; }

		/// <summary>
		/// Название документа.
		/// </summary>
		public string DocumentName { get; set; }

		/// <summary>
		/// Майм-тип документа.
		/// </summary>
		public string DocumentMimeType { get; set; }

		/// <summary>
		/// Идентификатор сертификата.
		/// </summary>
		public Guid CertificateId { get; set; }

		/// <summary>
		/// Текст для пользователя.
		/// </summary>
		public string TextForUser { get; set; }
	}

	/// <summary>
	/// Параметры получения тела сертификата.
	/// </summary>
	public class GetCertificateViewModel
	{
		/// <summary>
		/// Идентификатор сертификата.
		/// </summary>
		public Guid CertificateId { get; set; }
	}

	/// <summary>
	/// Параметры получения результата транзакции.
	/// </summary>
	public class GetTransactionInfoViewModel
	{
		/// <summary>
		/// Идентификатор транзакции.
		/// </summary>
		public Guid TransactionId { get; set; }
	}
}