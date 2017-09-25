using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletsServices.Core.DataInterfaces.Utilities
{
	/// <summary>
	/// Утилита для реализации криптографических алгоритмов.
	/// </summary>
	public interface ICryptographyUtility
	{
		/// <summary>
		/// Найти сертификат в реестре по отпечатку.
		/// </summary>
		/// <param name="thumbprint">Отпечаток сертификата.</param>
		/// <returns>Найденный сертификат.</returns>
		X509Certificate2 FindCertificate(string thumbprint);

		/// <summary>
		/// Подписать сообщение в формате PKCS#7.
		/// </summary>
		/// <param name="data">Сообщение для подписи.</param>
		/// <param name="certificate">Сертификат, подписывающий сообщение.
		/// Если параметр пустой, используется системный, который для подписи запросов в 1С.</param>
		/// <param name="isDetached">Отсоединять подпись.</param>
		/// <returns>Подпись в формате PKCS#7.</returns>
		byte[] Sign(Byte[] data, X509Certificate2 certificate = null, bool isDetached = false);

		/// <summary>
		/// Проверить сообщение, подписанное ЭП в формате PKCS#7.
		/// </summary>
		/// <param name="data">Оригинал сообщения.</param>
		/// <param name="encodedSignature">Подписанное сообщение/отделенная подпись в формате PKCS#7.</param>
		/// <param name="isDetached">Отсоединенная ли подпись.</param>
		/// <returns>Валидна ли подпись сообщения, т.е. не изменял ли его кто и правда ли оно принадлежит тому, кто говорит, что подписал его.</returns>
		bool Verify(Byte[] data, byte[] encodedSignature, bool isDetached = false);
	}
}
