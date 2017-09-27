(function () {
	'use strict';

	angular
		.module('crypto.services')
		.factory('C1Service', ['$resource', function ($resource) {
			return $resource('/api/C1/:id', null, {
				getCertificates: {
					url: '/api/C1/GetCertificates/:msisdn',
					method: 'POST',
					isArray: false,
				},
				getTransactionInfo: {
					url: '/api/C1/GetTransactionInfo/:transactionId',
					method: 'POST',
					isArray: false,
				},
				generateCertificate: {
					url: '/api/C1/GenerateCertificate',
					method: 'POST',
					isArray: false,
				},
				
			});
		}]);
})();

