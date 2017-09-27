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
	public class GetTransactionInfoViewModel
	{
		public Guid TransactionId { get; set; }
	}
}