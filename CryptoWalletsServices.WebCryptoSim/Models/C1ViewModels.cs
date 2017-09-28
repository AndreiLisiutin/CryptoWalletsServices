using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoWalletsServices.WebCryptoSim.Models
{
	public class GetCertificatesViewModel
	{
		public string Msisdn { get; set; }
	}
	public class ActivateViewModel
	{
		public string Msisdn { get; set; }
		public string Iccid { get; set; }
	}
	public class AuthenticateViewModel
	{
		public Guid CertificateId { get; set; }
		public string TextForUser { get; set; }
	}
	public class SignViewModel
	{
		public string DocumentBase64Data { get; set; }
		public string DocumentName { get; set; }
		public string DocumentMimeType { get; set; }
		public Guid CertificateId { get; set; }
		public string TextForUser { get; set; }
	}
	public class GetCertificateViewModel
	{
		public Guid CertificateId { get; set; }
	}
	public class GetTransactionInfoViewModel
	{
		public Guid TransactionId { get; set; }
	}
}