'use strict';

angular.module('crypto.controllers')
	.config([
		'$stateProvider',
		function ($stateProvider) {
			$stateProvider
				.state('crypto.navigation.getCertificates', {
					url: '/getCertificates',
					templateUrl: 'app/components/get-certificates/get-certificates.html',
					controller: 'GetCertificatesController'
				});
		}
	])
	.controller('GetCertificatesController', [
		'$scope', '$state', 'toastr', 'C1Service',
		function ($scope, $state, toastr, C1Service) {
			$scope.parameters = {
				msisdn: '79584066545'
			};
			$scope.doTest = function () {
				C1Service.getCertificates($scope.parameters).$promise
					.then(function (data) {
						toastr.success(data && data.transactionGuid, 'Успешно');
					})
					.catch(function (error) {
						toastr.error(error && error.data && error.data.message, 'Ошибка');
					});
			}
		}
	]);