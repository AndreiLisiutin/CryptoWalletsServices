using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletsServices.Core.Models.Business
{
	/// <summary>
	/// Модель ответа на запрос без результатов ответа 1С.
	/// </summary>
	public class C1Rescponse
	{
		/// <summary>
		/// Идентификатор транзакции 1С.
		/// </summary>
		public Guid TransactionGuid { get; set; }

		/// <summary>
		/// Код статуса транзакции.
		/// </summary>
		public int StatusCode { get; set; }

		/// <summary>
		/// Статуст транзакции.
		/// </summary>
		public string Description { get; set; }
	}

	/// <summary>
	/// Модель ответа на запрос 1С.
	/// </summary>
	/// <typeparam name="T">Основная информация ответа 1С.</typeparam>
	public class C1Rescponse<T>: C1Rescponse
	{
		public C1Rescponse()
		{

		}
		public C1Rescponse(C1Rescponse response)
		{
			this.Data = default(T);
			this.Description = response.Description;
			this.StatusCode = response.StatusCode;
			this.TransactionGuid = response.TransactionGuid;
		}

		/// <summary>
		/// Данные, возвращаемые 1С.
		/// </summary>
		public T Data { get; set; }
	}
}
