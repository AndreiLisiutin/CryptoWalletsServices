using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletsServices.Core.Models.Business
{
	public class FinanceDocument
	{
		public FinanceDocument(string base64Data, string name, string mimeType)
		{
			this.Data = base64Data == null ? null : Convert.FromBase64String(base64Data);
			this.Name = name;
			this.MimeType = mimeType;
		}

		public byte[] Data { get; private set; }
		public string Name { get; private set; }
		public string MimeType { get; private set; }
	}
}
