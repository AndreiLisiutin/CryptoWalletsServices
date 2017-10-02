using CryptoWalletsServices.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletsServices.Core.DataInterfaces.Repositories
{
	/// <summary>
	/// Репозиторий работы с 1С.
	/// </summary>
	public interface IC1Repository
	{
		/// <summary>
		/// Отправить запрос на подпись документа.
		/// </summary>
		/// <param name="document">Документ.</param>
		/// <param name="certificateId">Идентификатор сертификата пользователя.</param>
		/// <param name="textForUser">текст для пользователя.</param>
		/// <returns>Ответ 1С.</returns>
		C1Rescponse Sign(FinanceDocument document, Guid certificateId, string textForUser);

		/// <summary>
		/// Отправить запрос на активацию пользователя.
		/// </summary>
		/// <param name="msisdn">Номер телефона.</param>
		/// <param name="iccid">ICCID симки.</param>
		/// <returns>Ответ 1С.</returns>
		C1Rescponse Activate(string msisdn, string iccid);

		/// <summary>
		/// Отправить запрос на генерацию сертификата пользователя.
		/// </summary>
		/// <param name="requestData">Данные пользователя.</param>
		/// <returns>Ответ 1С.</returns>
		C1Rescponse GenerateCertificate(GenerateCertificateRequest requestData);

		/// <summary>
		/// Отправить запрос на получение списка сертификатов телефона.
		/// </summary>
		/// <param name="msisdn">Номер телефона.</param>
		/// <returns>Ответ 1С.</returns>
		C1Rescponse GetCertificates(string msisdn);

		/// <summary>
		/// Отправить запрос на получение тела сертификата пользователя.
		/// </summary>
		/// <param name="certificateId">Идентификатор сертификата пользователя.</param>
		/// <returns>Ответ 1С.</returns>
		C1Rescponse GetCertificate(Guid certificateId);

		/// <summary>
		/// Отправить запрос на аутентификацию в 1С.
		/// </summary>
		/// <param name="certificateId">Идентификатор сертификата пользователя.</param>
		/// <param name="textForUser">Текст для пользователя.</param>
		/// <returns>Ответ 1С.</returns>
		C1Rescponse Authenticate(Guid certificateId, string textForUser);

		/// <summary>
		/// Отправить запрос на запрос доступа к сертификату пользователя в 1С.
		/// </summary>
		/// <param name="msisdn">Номер телефона.</param>
		/// <returns>Ответ 1С.</returns>
		C1Rescponse RequestAccessToCertificate(string msisdn);

		/// <summary>
		/// Получить инфу о запросе, ранее отправленном в 1С.
		/// </summary>
		/// <typeparam name="T">Тип возвращаемого 1С значения.</typeparam>
		/// <param name="transactionId">Идентификатор запроса.</param>
		/// <returns>Результат запроса в 1С.</returns>
		C1Rescponse<T> GetTransactionInfo<T>(Guid transactionId);
	}
}
