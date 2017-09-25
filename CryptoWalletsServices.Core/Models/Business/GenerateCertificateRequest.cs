using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletsServices.Core.Models.Business
{
	/// <summary>
	/// Модель для генерации сертификата.
	/// </summary>
	public class GenerateCertificateRequest
	{
		/// <summary>
		/// Номер телефона.
		/// </summary>
		public string Msisdn { get; set; }
		/// <summary>
		/// Полное имя, ФИО.
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// Фамилия.
		/// </summary>
		public string Surname { get; set; }

		/// <summary>
		/// Имя отчество.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Страна. Пока что передавать просто RU.
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// Регион.
		/// </summary>
		public string Region { get; set; }

		/// <summary>
		/// Город.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// ИНН.
		/// </summary>
		public string Inn { get; set; }

		/// <summary>
		/// СНИЛС.
		/// </summary>
		public string Snils { get; set; }

		/// <summary>
		/// Почта.
		/// </summary>
		public string Email { get; set; }
	}
}
