﻿using CryptoWalletsServices.Core.DataInterfaces.Repositories;
using CryptoWalletsServices.Core.Models.Business;
using CryptoWalletsServices.Core.ServiceInterfaces;
using CryptoWalletsServices.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletsServices.Core.Services
{

	public class C1Service: IC1Service
	{
		private IC1Repository c1Repository;

		public C1Service(IC1Repository c1Repository)
		{
			this.c1Repository = c1Repository;
		}

		public C1Rescponse<List<Guid>> GetCertificates(string msisdn)
		{
			Argument.Require(!string.IsNullOrWhiteSpace(msisdn), "Номер телефона пустой.");
			var response = c1Repository.GetCertificates(msisdn);
			return response;
		}

		public C1Rescponse<Guid> GenerateCertificate(GenerateCertificateRequest requestData)
		{
			Argument.Require(requestData != null, "Данные для генерации сертификата пустые.");
			Argument.Require(!string.IsNullOrWhiteSpace(requestData.Msisdn), "Номер телефона не задан.");
			Argument.Require(!string.IsNullOrWhiteSpace(requestData.FullName), "ФИО не задано.");
			Argument.Require(!string.IsNullOrWhiteSpace(requestData.City), "Город не задан.");
			Argument.Require(!string.IsNullOrWhiteSpace(requestData.Country), "Страна не задана.");
			Argument.Require(!string.IsNullOrWhiteSpace(requestData.Email), "Email не задан.");
			Argument.Require(!string.IsNullOrWhiteSpace(requestData.Inn), "ИНН не задан.");
			Argument.Require(!string.IsNullOrWhiteSpace(requestData.Name), "ИО не заданы.");
			Argument.Require(!string.IsNullOrWhiteSpace(requestData.Region), "Регион не задан.");
			Argument.Require(!string.IsNullOrWhiteSpace(requestData.Snils), "СНИЛС не задан.");
			Argument.Require(!string.IsNullOrWhiteSpace(requestData.Surname), "Фамилия не задана.");

			var response = c1Repository.GenerateCertificate(requestData);
			return response;
		}

		public C1Rescponse<T> GetTransactionInfo<T>(Guid transactionId)
		{
			Argument.Require(transactionId != Guid.Empty, "Идентификатор транзакции пустой.");
			var response = c1Repository.GetTransactionInfo<T>(transactionId);
			return response;
		}

		public C1Rescponse<bool> Activate(string msisdn, string iccid)
		{
			Argument.Require(!string.IsNullOrWhiteSpace(msisdn), "Номер телефона пустой.");
			Argument.Require(!string.IsNullOrWhiteSpace(iccid), "Идентификатор телефона (iccid) пустой.");
			var response = c1Repository.Activate(msisdn, iccid);
			return response;
		}


	}
}