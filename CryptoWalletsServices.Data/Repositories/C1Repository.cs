using CryptoWalletsServices.Core.DataInterfaces.Repositories;
using CryptoWalletsServices.Core.DataInterfaces.Utilities;
using CryptoWalletsServices.Core.Models.Business;
using CryptoWalletsServices.Utils;
using CryptoWalletsServices.Utils.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletsServices.Data.Repositories
{
	/// <summary>
	/// Репозиторий 1С.
	/// </summary>
	public class C1Repository : IC1Repository
	{
		private readonly string _1C_SERVICE_API_URL = "1C_ApiUrl";
		private readonly string _1C_SERVICE_ENCODING = "1C_Encoding";
		private readonly string _1C_CERTIFICATE_TEMPLATE = "1C_CertificateTemplate";
		private readonly string _1C_CERTIFICATE_AUTHORITY = "1C_CertificateAuthority";
		private readonly string _1C_CERTIFICATE_EXCLUSIVE_ACCESS = "1C_ExclusiveAccess";
		/// <summary>
		/// Рест-клиент к сервису 1С. Ленивая версия.
		/// </summary>
		private Lazy<RestClient> _serviceApiClient;

		/// <summary>
		/// Утилита криптографии.
		/// </summary>
		protected ICryptographyUtility CryptographyUtility { get; private set; }

		/// <summary>
		/// Конфигурация системы.
		/// </summary>
		protected Configuration Configuration { get; private set; }

		/// <summary>
		/// Рест-клиент к сервису 1С.
		/// </summary>
		protected RestClient ServiceApiClient => this._serviceApiClient.Value;

		/// <summary>
		/// Кодировка, используемая в 1С.
		/// </summary>
		protected Encoding ServiceApiEncoding { get; private set; }

		/// <summary>
		/// Шаблон для генерации пользовательского сертификата.
		/// </summary>
		protected Guid ServiceCertificateTemplate { get; private set; }

		/// <summary>
		/// УЦ для генерации пользовательского сертификата.
		/// </summary>
		protected Guid ServiceAuthority { get; private set; }

		/// <summary>
		/// Будут ли при генерации у пользователей эксклюзивные доступы к сертификатам.
		/// </summary>
		protected bool ServiceCertificateHasExclusiveAccess { get; private set; }

		/// <summary>
		/// Репозиторий 1С.
		/// </summary>
		/// <param name="cryptographyUtility">Утилита для работы с криптографией.</param>
		/// <param name="configuration">Конфигурация системы.</param>
		public C1Repository(ICryptographyUtility cryptographyUtility, Configuration configuration)
		{
			this.CryptographyUtility = cryptographyUtility;
			this.Configuration = configuration;
			this.ServiceApiEncoding = Encoding.GetEncoding(_GetConfig(_1C_SERVICE_ENCODING));
			this.ServiceCertificateTemplate = Guid.Parse(_GetConfig(_1C_CERTIFICATE_TEMPLATE));
			this.ServiceAuthority = Guid.Parse(_GetConfig(_1C_CERTIFICATE_AUTHORITY));
			this.ServiceCertificateHasExclusiveAccess = bool.Parse(_GetConfig(_1C_CERTIFICATE_EXCLUSIVE_ACCESS));
			this._serviceApiClient = new Lazy<RestClient>(() => new RestClient(_GetConfig(_1C_SERVICE_API_URL)), true);
		}

		/// <summary>
		/// Получить значение в конфигурации по ключу.
		/// </summary>
		/// <param name="configName">Ключ.</param>
		/// <returns>Значение.</returns>
		private string _GetConfig(string configName)
		{
			Argument.Require(this.Configuration != null, "Не найдена конфигурация.");
			var configItem = this.Configuration.AppSettings.Settings[configName];
			Argument.Require(configItem != null,
				$"Некорректно задана конфигурация проекта. Не определен ключ {configName}.");
			string configValue = configItem.Value;
			Argument.Require(configValue != null,
				$"Некорректно задана конфигурация проекта. Пареметр {configName} пустой.");
			return configValue;
		}

		/// <summary>
		/// Выполнить запрос к 1С.
		/// </summary>
		/// <typeparam name="T">Уберем потом.</typeparam>
		/// <param name="url">Относительный URL запроса.</param>
		/// <param name="data">Данные, передаваемые ва 1С.</param>
		/// <param name="requestTransform">Если что-то с умолчаниями запроса нужно поменять.</param>
		/// <param name="method">Метод запроса.</param>
		/// <returns>Информация о транзакции.</returns>
		protected C1Rescponse<T> Request<T>(string url, object data, Action<RestRequest> requestTransform = null, Method method = Method.POST)
		{
			Argument.Require(!string.IsNullOrEmpty(url), "URL метода пустой.");
			Argument.Require(data != null, "Переданные данные пустые.");
			string message = JsonConvert.SerializeObject(data);

			byte[] msgBytes = this.ServiceApiEncoding.GetBytes(message);
			byte[] encodedSignature = CryptographyUtility.Sign(msgBytes);

			RestRequest request = new RestRequest(url, method);
			request.AddHeader("Content-type", "multipart/form-data");
			request.AddFileBytes("request", encodedSignature, "request.json", "application/json");
			requestTransform?.Invoke(request);
			IRestResponse<C1Rescponse<T>> response = this.ServiceApiClient.Execute<C1Rescponse<T>>(request);
			Argument.Require(IsSuccessStatusCode(response.StatusCode), "Возникла ошибка при выполнении запроса к 1С.", new Exception(response.Content));

			return response.Data;
		}

		/// <summary>
		/// Успешный ли код веб-ответа.
		/// </summary>
		/// <param name="statusCode">Код веб-ответа.</param>
		/// <returns>Успешный ли код веб-ответа.</returns>
		private bool IsSuccessStatusCode(HttpStatusCode statusCode)
		{
			return ((int)statusCode >= 200) && ((int)statusCode <= 299);
		}

		/// <summary>
		/// Отправить запрос на подпись документа.
		/// </summary>
		/// <param name="document">Документ.</param>
		/// <param name="certificateId">Идентификатор сертификата пользователя.</param>
		/// <param name="textForUser">текст для пользователя.</param>
		/// <returns>Ответ 1С.</returns>
		public C1Rescponse Sign(FinanceDocument document, Guid certificateId, string textForUser)
		{
			var parameters = new
			{
				certificateGuid = certificateId,
				text = textForUser,
				callbackUri = ""
			};

			C1Rescponse<string> response = this.Request<string>("/Sign/Sign", parameters,
				r => r.AddFileBytes("data", document.Data, document.Name, document.MimeType));
			return response;
		}

		/// <summary>
		/// Отправить запрос на активацию пользователя.
		/// </summary>
		/// <param name="msisdn">Номер телефона.</param>
		/// <param name="iccid">ICCID симки.</param>
		/// <returns>Ответ 1С.</returns>
		public C1Rescponse Activate(string msisdn, string iccid)
		{
			var parameters = new
			{
				MSISDN = msisdn,
				ICCID = iccid,
				callbackUri = ""
			};

			C1Rescponse<bool> response = this.Request<bool>("/Activation/Activate", parameters);
			return response;
		}

		/// <summary>
		/// Отправить запрос на генерацию сертификата пользователя.
		/// </summary>
		/// <param name="requestData">Данные пользователя.</param>
		/// <returns>Ответ 1С.</returns>
		public C1Rescponse GenerateCertificate(GenerateCertificateRequest requestData)
		{
			var parameters = new
			{
				MSISDN = requestData.Msisdn,
				exclusiveAccess = ServiceCertificateHasExclusiveAccess,
				authorityGuid = ServiceAuthority,
				templateGuid = ServiceCertificateTemplate,
				subject = new Dictionary<string, string>
				{
					["2.5.4.3"] = requestData.FullName,
					["2.5.4.4"] = requestData.Surname,
					["2.5.4.42"] = requestData.Name,
					["2.5.4.6"] = requestData.Country,
					["2.5.4.8"] = requestData.Region,
					["2.5.4.7"] = requestData.City,
					["1.2.643.100.3"] = requestData.Inn,
					["1.2.643.3.131.1.1"] = requestData.Snils,
					["1.2.840.113549.1.9.1"] = requestData.Email,
				}
			};

			C1Rescponse<Guid> response = this.Request<Guid>("/Certificates/Generate", parameters);
			return response;
		}

		/// <summary>
		/// Отправить запрос на получение списка сертификатов телефона.
		/// </summary>
		/// <param name="msisdn">Номер телефона.</param>
		/// <returns>Ответ 1С.</returns>
		public C1Rescponse GetCertificates(string msisdn)
		{
			var parameters = new
			{
				MSISDN = msisdn,
				callbackUri = ""
			};

			C1Rescponse<List<Guid>> response = this.Request<List<Guid>>("/Certificates/GetCertificates", parameters);
			return response;
		}

		/// <summary>
		/// Отправить запрос на получение тела сертификата пользователя.
		/// </summary>
		/// <param name="certificateId">Идентификатор сертификата пользователя.</param>
		/// <returns>Ответ 1С.</returns>
		public C1Rescponse GetCertificate(Guid certificateId)
		{
			var parameters = new
			{
				certificateGuid = certificateId,
				callbackUri = ""
			};

			C1Rescponse<string> response = this.Request<string>("/Certificates/GetCertificate", parameters);
			return response;
		}

		/// <summary>
		/// Отправить запрос на аутентификацию в 1С.
		/// </summary>
		/// <param name="certificateId">Идентификатор сертификата пользователя.</param>
		/// <param name="textForUser">Текст для пользователя.</param>
		/// <returns>Ответ 1С.</returns>
		public C1Rescponse RequestAccessToCertificate(string msisdn)
		{
			var parameters = new
			{
				MSISDN = msisdn
			};

			C1Rescponse<Guid> response = this.Request<Guid>("/Certificates/RequestAccess", parameters);
			return response;
		}

		/// <summary>
		/// Отправить запрос на запрос доступа к сертификату пользователя в 1С.
		/// </summary>
		/// <param name="msisdn">Номер телефона.</param>
		/// <returns>Ответ 1С.</returns>
		public C1Rescponse Authenticate(Guid certificateId, string textForUser)
		{
			var parameters = new
			{
				certificateGuid = certificateId,
				text = textForUser,
				callbackUri = ""
			};

			C1Rescponse<string> response = this.Request<string>("/Authentication/Authenticate", parameters);
			return response;
		}

		/// <summary>
		/// Получить инфу о запросе, ранее отправленном в 1С.
		/// </summary>
		/// <typeparam name="T">Тип возвращаемого 1С значения.</typeparam>
		/// <param name="transactionId">Идентификатор запроса.</param>
		/// <returns>Результат запроса в 1С.</returns>
		public C1Rescponse<T> GetTransactionInfo<T>(Guid transactionId)
		{
			var parameters = new
			{
				transactionGuid = transactionId
			};
			C1Rescponse<T> response = this.Request<T>("/Transactions/GetTransactionInfo", parameters);
			return response;
		}
	}
}
