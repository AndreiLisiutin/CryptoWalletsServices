'use strict';

angular.module('crypto.controllers')
	.config([
		'$stateProvider',
		function ($stateProvider) {
			$stateProvider
				.state('crypto.navigation.activate', {
					url: '/activate',
					templateUrl: 'app/components/activate/activate.html',
					controller: 'ActivateController'
				});
		}
	])
	.controller('ActivateController', [
		'$scope', '$state', 'toastr', 'C1Service',
		function ($scope, $state, toastr, C1Service) {
			$scope.parameters = {
				msisdn: '79584066545',
				iccid: '8974201702070610452'
			};
			$scope.transaction = {};
			$scope.transactionId = null;
			$scope.doTest = doTest;
			$scope.getTransactionInfo = getTransactionInfo;

			////////////////////////////////

			function doTest() {
				$scope.transactionId = null;
				C1Service.activate($scope.parameters).$promise
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