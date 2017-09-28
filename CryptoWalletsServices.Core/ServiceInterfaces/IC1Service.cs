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
		C1Rescponse<List<Guid>> GetCertificates(string msisdn);

		C1Rescponse<Guid> GenerateCertificate(GenerateCertificateRequest requestData);

		C1Rescponse<T> GetTransactionInfo<T>(Guid transactionId);

		C1Rescponse<bool> Activate(string msisdn, string iccid);

		C1Rescponse<string> Authenticate(Guid certificateId, string textForUser);

		C1Rescponse<string> Sign(FinanceDocument document, Guid certificateId, string textForUser);

		C1Rescponse<string> GetCertificate(Guid certificateId);
	}
}
