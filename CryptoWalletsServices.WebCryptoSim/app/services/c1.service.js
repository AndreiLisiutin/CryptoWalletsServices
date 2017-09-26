(function () {
	'use strict';

	angular
		.module('crypto.services')
		.factory('C1Service', ['$resource', function ($resource) {
			return $resource('/api/C1/:id', null, {
				getCertificates: {
					url: '/api/C1/GetCertificates/:msisdn',
					method: 'GET',
					isArray: false,
				},
				
			});
		}]);
})();

