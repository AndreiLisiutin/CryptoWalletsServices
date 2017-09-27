'use strict';

angular.module('crypto.controllers')
	.config([
		'$stateProvider',
		function ($stateProvider) {
			$stateProvider
				.state('crypto.navigation.generateCertificate', {
					url: '/generateCertificate',
					templateUrl: 'app/components/generate-certificate/generate-certificate.html',
					controller: 'GenerateCertificateController'
				});
		}
	])
	.controller('GenerateCertificateController', [
		'$scope', '$state', 'toastr', 'C1Service',
		function ($scope, $state, toastr, C1Service) {
			$scope.parameters = {
				msisdn: '79584066545',
				fullName: 'Лисютин Андрей Петрович',
				surname: 'Лисютин',
				name: 'Андрей Петрович',
				country: 'RU',
				region: 'Татарстан',
				city: 'Казань',
				inn: '1234567890',
				snils: '123-456-789 01',
				email: 'lisutin.andrey@gmail.com'
			};
			$scope.transaction = {};
			$scope.transactionId = null;
			$scope.doTest = doTest;
			$scope.getTransactionInfo = getTransactionInfo;

			////////////////////////////////

			function doTest() {
				$scope.transactionId = null;
				C1Service.generateCertificate($scope.parameters).$promise
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
			};
		}
	]);