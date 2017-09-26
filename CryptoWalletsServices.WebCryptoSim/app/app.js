'use strict';

angular.module('crypto.extensions', [
	'uiRouterStyles',
	'ngSanitize',
	'ui.router',
	'ngAnimate',
	'toastr'
]);
angular.module('crypto.services', [
	'ngResource'
]);
angular.module('crypto.controllers', ['crypto.extensions', 'crypto.services']);

angular.module('cryptoApp', ['crypto.controllers', 'crypto.extensions', 'crypto.services'])
	.run(['$rootScope',
		function ($rootScope) {
			$rootScope.$on('$viewContentLoaded', function () {
				$('html, body').animate({scrollTop: 0}, 200);
			});
		}
	])
	.config([
		'$stateProvider', '$locationProvider', '$urlRouterProvider',
		function ($stateProvider, $locationProvider, $urlRouterProvider) {
			// для маршрутов со слешом
			$locationProvider.hashPrefix('');

			// маршрут по умолчанию.
			$urlRouterProvider.otherwise('/getCertificates');

			//базовый путь маршрутов приложения
			$stateProvider
				.state('crypto',
					{
						url: '^',
						abstract: true,
						templateUrl: 'app/components/_layout/main.html'
					});
		}
	])
	.config([
		'$httpProvider',
		function ($httpProvider) {
			/**
			 * Приведение к camelCase объекта рекурсивно.
			 * @param data объект.
			 * @returns {*}
			 */
			function toCamelCase(data) {
				if (!data) {
					return data;
				}
				var camelCaseObject = {};
				_.forEach(
					data,
					function (value, key) {
						if (_.isPlainObject(value) || _.isArray(value)) {
							value = toCamelCase(value);
						}
						camelCaseObject[_.camelCase(key)] = value;
					}
				);
				return camelCaseObject;
			}

			$httpProvider.interceptors.push([
				'$rootScope', '$q', '$window', '$timeout',
				function ($rootScope, $q, $window, $timeout) {
					return {
						'response': function (response) {
							return toCamelCase(response);
						},

						'responseError': function (rejection) {
							return $q.reject(toCamelCase(rejection));
						}
					};
				}
			]);
		}
	]);