using CryptoWalletsServices.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletsServices.Core.DataInterfaces.Repositories
{
	public interface IC1Repository
	{
		C1Rescponse<string> Sign(FinanceDocument document, Guid certificateId, string textForUser);

		C1Rescponse<bool> Activate(string msisdn, string iccid);

		C1Rescponse<Guid> GenerateCertificate(GenerateCertificateRequest requestData);

		C1Rescponse<List<Guid>> GetCertificates(string msisdn);

		C1Rescponse<string> GetCertificate(Guid certificateId);

		C1Rescponse<string> Authenticate(Guid certificateId, string textForUser);

		C1Rescponse<Guid> RequestAccessToCertificate(string msisdn);

		C1Rescponse<T> GetTransactionInfo<T>(Guid transactionId);
	}
}
