(function () {
	'use strict';

	angular
		.module('crypto.services')
		.factory('C1Service', ['$resource', function ($resource) {
			return $resource('/api/C1/:id', null, {
				getCertificates: {
					url: '/api/C1/GetCertificates/:msisdn',
					method: 'POST',
					isArray: false
				},
				generateCertificate: {
					url: '/api/C1/GenerateCertificate',
					method: 'POST',
					isArray: false
				},
				activate: {
					url: '/api/C1/Activate',
					method: 'POST',
					isArray: false
				},
				getCertificate: {
					url: '/api/C1/GetCertificate',
					method: 'POST',
					isArray: false
				},
				authenticate: {
					url: '/api/C1/Authenticate',
					method: 'POST',
					isArray: false
				},
				sign: {
					url: '/api/C1/Sign',
					method: 'POST',
					isArray: false
				},
				//----------------------------Получение результатов-------------------------------
				getTransactionInfo: {
					url: '/api/C1/GetTransactionInfo/:transactionId',
					method: 'POST',
					isArray: false
				}
				
			});
		}]);
})();

