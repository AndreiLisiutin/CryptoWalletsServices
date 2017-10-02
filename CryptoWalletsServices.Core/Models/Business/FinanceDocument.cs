using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletsServices.Core.Models.Business
{
	/// <summary>
	/// Финансовое распорядение. Просто документ, короче.
	/// </summary>
	public class FinanceDocument
	{
		/// <summary>
		/// Финансовое распорядение. Просто документ, короче.
		/// </summary>
		/// <param name="base64Data">Сериализованный документ в base64.</param>
		/// <param name="name">Название.</param>
		/// <param name="mimeType">Mime-тип.</param>
		public FinanceDocument(string base64Data, string name, string mimeType)
		{
			this.Data = base64Data == null ? null : Convert.FromBase64String(base64Data);
			this.Name = name;
			this.MimeType = mimeType;
		}

		/// <summary>
		/// Сериализованный документ в массиве байтов.
		/// </summary>
		public byte[] Data { get; private set; }

		/// <summary>
		/// Название.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Mime-тип.
		/// </summary>
		public string MimeType { get; private set; }
	}
}
