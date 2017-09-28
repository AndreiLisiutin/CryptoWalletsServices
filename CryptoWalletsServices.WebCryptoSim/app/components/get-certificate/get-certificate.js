'use strict';

angular.module('crypto.controllers')
	.config([
		'$stateProvider',
		function ($stateProvider) {
			$stateProvider
				.state('crypto.navigation.getCertificate', {
					url: '/getCertificate',
					templateUrl: 'app/components/get-certificate/get-certificate.html',
					controller: 'GetCertificateController'
				});
		}
	])
	.controller('GetCertificateController', [
		'$scope', '$state', 'toastr', 'C1Service',
		function ($scope, $state, toastr, C1Service) {
			$scope.parameters = {
				certificateId: '0d3231c2-dc41-44aa-88c4-48a787d1258e'
			};
			$scope.transaction = {};
			$scope.transactionId = null;
			$scope.doTest = doTest;
			$scope.getTransactionInfo = getTransactionInfo;

			////////////////////////////////

			function doTest() {
				$scope.transactionId = null;
				C1Service.getCertificate($scope.parameters).$promise
					.then(function (data) {
						$scope.transactionId = data.transactionGuid;
						getTransactionInfo();
					})
					.catch(function (error) {
						toastr.error(error && error.data && error.data.message, 'Ошибка');
					});
			}

			function getTransactionInfo() {
				C1Service.getTransactionInfo({transactionId: $scope.transactionId}).$promise
					.then(function (data) {
						$scope.transaction = data;
					})
					.catch(function (error) {
						toastr.error(error && error.data && error.data.message, 'Ошибка');
					});
			}
		}
	]);