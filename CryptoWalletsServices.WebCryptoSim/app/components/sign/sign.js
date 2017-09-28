'use strict';

angular.module('crypto.controllers')
	.config([
		'$stateProvider',
		function ($stateProvider) {
			$stateProvider
				.state('crypto.navigation.sign', {
					url: '/sign',
					templateUrl: 'app/components/sign/sign.html',
					controller: 'SignController'
				});
		}
	])
	.controller('SignController', [
		'$scope', '$state', 'toastr', 'C1Service',
		function ($scope, $state, toastr, C1Service) {
			$scope.document = {
				filesize: null,
				filetype: null,
				filename: null,
				base64: null
			};
			$scope.parameters = {
				documentBase64Data: null,
				documentName: '',
				documentMimeType: '',
				certificateId: '0d3231c2-dc41-44aa-88c4-48a787d1258e',
				textForUser: 'Подпиши, блеять!'
			};
			$scope.transaction = {};
			$scope.transactionId = null;
			$scope.doTest = doTest;
			$scope.getTransactionInfo = getTransactionInfo;

			////////////////////////////////

			function doTest() {
				$scope.transactionId = null;
				debugger;
				$scope.parameters.documentBase64Data = $scope.document.base64;
				$scope.parameters.documentName = $scope.document.filename;
				$scope.parameters.documentMimeType = $scope.document.filetype;
				C1Service.sign($scope.parameters).$promise
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