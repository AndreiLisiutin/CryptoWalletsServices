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
	public class C1Repository : IC1Repository
	{
		private readonly string _1C_SERVICE_API_URL = "1C_ApiUrl";
		private readonly string _1C_SERVICE_ENCODING = "1C_Encoding";
		private readonly string _1C_CERTIFICATE_TEMPLATE = "1C_CertificateTemplate";
		private readonly string _1C_CERTIFICATE_AUTHORITY = "1C_CertificateAuthority";
		private readonly string _1C_CERTIFICATE_EXCLUSIVE_ACCESS = "1C_ExclusiveAccess";
		private Lazy<RestClient> _serviceApiClient;

		protected ICryptographyUtility CryptographyUtility { get; private set; }
		protected Configuration Configuration { get; private set; }
		protected RestClient ServiceApiClient => this._serviceApiClient.Value;
		protected Encoding ServiceApiEncoding { get; private set; }
		protected Guid ServiceCertificateTemplate { get; private set; }
		protected Guid ServiceAuthority { get; private set; }
		protected bool ServiceCertificateHasExclusiveAccess { get; private set; }

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

		public bool IsSuccessStatusCode(HttpStatusCode statusCode)
		{
			return ((int)statusCode >= 200) && ((int)statusCode <= 299);
		}

		public C1Rescponse<string> Sign(byte[] file, Guid certificateId, string textForUser)
		{
			var parameters = new
			{
				certificateGuid = certificateId,
				text = textForUser,
				callbackUri = ""
			};

			C1Rescponse<string> response = this.Request<string>("/Sign/Sign", parameters,
				r => r.AddFileBytes("data", file, "pdfsample.pdf", "application/pdf"));
			return response;
		}

		public C1Rescponse<bool> Activate(string msisdn, string iccid)
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

		public C1Rescponse<Guid> GenerateCertificate(GenerateCertificateRequest requestData)
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

		public C1Rescponse<List<Guid>> GetCertificates(string msisdn)
		{
			var parameters = new
			{
				MSISDN = msisdn,
				callbackUri = ""
			};

			C1Rescponse<List<Guid>> response = this.Request<List<Guid>>("/Certificates/GetCertificates", parameters);
			return response;
		}

		public C1Rescponse<string> GetCertificate(Guid certificateId)
		{
			var parameters = new
			{
				certificateGuid = certificateId,
				callbackUri = ""
			};

			C1Rescponse<string> response = this.Request<string>("/Certificates/GetCertificate", parameters);
			return response;
		}

		public C1Rescponse<Guid> RequestAccessToCertificate(string msisdn)
		{
			var parameters = new
			{
				MSISDN = msisdn
			};

			C1Rescponse<Guid> response = this.Request<Guid>("/Certificates/RequestAccess", parameters);
			return response;
		}
		
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
