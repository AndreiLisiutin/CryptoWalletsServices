using CryptoWalletsServices.Core.DataInterfaces.Utilities;
using CryptoWalletsServices.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletsServices.Data.Utilities
{
	/// <summary>
	/// Утилита для реализации криптографических алгоритмов.
	/// </summary>
	public class CryptographyUtility : ICryptographyUtility
	{
		private readonly string _1C_SERVICE_CERTIFICATE_THUMBPRINT = "1C_CertificateThumbprint";
		private Lazy<X509Certificate2> _serviceApiCertificate;

		protected Configuration Configuration { get; private set; }
		protected X509Certificate2 ServiceApiCertificate => this._serviceApiCertificate.Value;

		public CryptographyUtility(Configuration configuration)
		{
			this.Configuration = configuration;
			this._serviceApiCertificate = new Lazy<X509Certificate2>(() => _GetServiceCertificate(), true);
		}

		/// <summary>
		/// Получить сертификат для подписи запросов в 1С.
		/// </summary>
		/// <returns>Сертификат для подписи запросов в 1С.</returns>
		private X509Certificate2 _GetServiceCertificate()
		{
			Argument.Require(this.Configuration != null, "Не найдена конфигурация.");
			var configItem = this.Configuration.AppSettings.Settings[_1C_SERVICE_CERTIFICATE_THUMBPRINT];
			Argument.Require(configItem != null,
				$"Некорректно задана конфигурация проекта. Не найден пареметр с отпечатком сертификата для подписи запросов 1С. Должен быть определен ключ {_1C_SERVICE_CERTIFICATE_THUMBPRINT}.");
			string thumbPrint = configItem.Value;
			Argument.Require(thumbPrint != null,
				$"Некорректно задана конфигурация проекта. Пареметр с отпечатком сертификата для подписи запросов 1С ({_1C_SERVICE_CERTIFICATE_THUMBPRINT}) пустой.");
			X509Certificate2 certificate = this.FindCertificate(thumbPrint);
			return certificate;
		}

		/// <summary>
		/// Найти сертификат в реестре по отпечатку.
		/// </summary>
		/// <param name="thumbprint">Отпечаток сертификата.</param>
		/// <returns>Найденный сертификат.</returns>
		public X509Certificate2 FindCertificate(string thumbprint)
		{
			X509Store storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			storeMy.Open(OpenFlags.ReadOnly);
			X509Certificate2 certificate = null;
			foreach (X509Certificate2 cert in storeMy.Certificates)
			{
				if (cert.Thumbprint.Equals(thumbprint, StringComparison.InvariantCulture))
				{
					certificate = cert;
					break;
				}
			}
			storeMy.Close();
			return certificate;
		}

		/// <summary>
		/// Подписать сообщение в формате PKCS#7.
		/// </summary>
		/// <param name="data">Сообщение для подписи.</param>
		/// <param name="certificate">Сертификат, подписывающий сообщение. 
		/// Если параметр пустой, используется системный, который для подписи запросов в 1С.</param>
		/// <param name="isDetached">Отсоединять подпись.</param>
		/// <returns>Подпись в формате PKCS#7.</returns>
		public byte[] Sign(Byte[] data, X509Certificate2 certificate = null, bool isDetached = false)
		{
			Argument.Require(data != null, "Сообщение для подписи пустое.");
			certificate = certificate ?? this.ServiceApiCertificate;

			ContentInfo contentInfo = new ContentInfo(data);
			SignedCms signedCms = new SignedCms(contentInfo, isDetached);
			CmsSigner cmsSigner = new CmsSigner(certificate);
			signedCms.ComputeSignature(cmsSigner);
			return signedCms.Encode();
		}

		/// <summary>
		/// Проверить сообщение, подписанное ЭП в формате PKCS#7.
		/// </summary>
		/// <param name="data">Оригинал сообщения.</param>
		/// <param name="encodedSignature">Подписанное сообщение/отделенная подпись в формате PKCS#7.</param>
		/// <param name="isDetached">Отсоединенная ли подпись.</param>
		/// <returns>Валидна ли подпись сообщения, т.е. не изменял ли его кто и правда ли оно принадлежит тому, кто говорит, что подписал его.</returns>
		public bool Verify(Byte[] data, byte[] encodedSignature, bool isDetached = false)
		{
			Argument.Require(data != null, "Оригинал сообщения пустой.");
			Argument.Require(encodedSignature != null, "Подписанное сообщение/отделенная подпись в формате PKCS#7 пустой.");

			ContentInfo contentInfo = new ContentInfo(data);
			SignedCms signedCms = new SignedCms(contentInfo, isDetached);
			signedCms.Decode(encodedSignature);

			try
			{
				signedCms.CheckSignature(true);
			}
			catch (System.Security.Cryptography.CryptographicException e)
			{
				return false;
			}

			return true;
		}
	}
}
