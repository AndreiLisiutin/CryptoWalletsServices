using CryptoWalletsServices.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletsServices.Core.ServiceInterfaces
{
	public interface IC1Service
	{
		C1Rescponse GetCertificates(string msisdn);

		C1Rescponse GenerateCertificate(GenerateCertificateRequest requestData);

		C1Rescponse<T> GetTransactionInfo<T>(Guid transactionId);

		C1Rescponse Activate(string msisdn, string iccid);

		C1Rescponse Authenticate(Guid certificateId, string textForUser);

		C1Rescponse Sign(FinanceDocument document, Guid certificateId, string textForUser);

		C1Rescponse GetCertificate(Guid certificateId);
	}
}
