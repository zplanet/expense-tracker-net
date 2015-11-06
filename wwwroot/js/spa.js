(function(){

	$(function() {
		$('.nav a').on('click', function(){ 
			if($('.navbar-toggle').css('display') !='none') {
				$(".navbar-toggle").trigger( "click" );
			}
		});
	});

	angular.module('ExpenseTrackerApp', ['ngRoute', 'chart.js'])

	.controller('MainController', function($scope, $route, $routeParams, $location, $http, $timeout) {

		$scope.$route = $route;
		$scope.$location = $location;
		$scope.$routeParams = $routeParams;

		$scope.message = '';
		$scope.messageStyle = '';
		
		$scope.email = '';
		$scope.password = '';
		$scope.user = '';
		
		$scope.showWarning = function(msg) {
			
			if (!msg || 0 === msg.length) {
				$scope.messageStyle = '';
			}
			else {
				$scope.messageStyle = 'alert-warning in';
			}
			
			$scope.message = msg;
		}

		var showMessage = function(msgStyle) {
			return function(msg, url) {
				$scope.message = msg;
				$scope.messageStyle = msgStyle;
				
				$timeout(function(){
					$scope.message = '';
					$scope.messageStyle = '';
					if ('undefined' !== typeof url) {
						$location.url(url);
					}
				}, 2000);
			}
		}

		$scope.showError = showMessage('alert-danger in');
		$scope.showSuccess = showMessage('alert-success in');

		$scope.login = function() {

			$http({
				method: 'post',
				url: 'api/users',
				data: {email: $scope.email, password: CryptoJS.SHA256($scope.password).toString()},
				headers: { "Content-Type": "application/json" }
			})
			.success(function(data, status, headers, config) {
				$scope.showSuccess('Welcome ' + $scope.email);
				$scope.setAuthorization($scope.email, data);
				$scope.clearLoginInfo();
			})
			.error(function(data, status, headers, config) {
				$scope.showError(data);
				$scope.clearLoginInfo();
			});
		}

		$scope.setAuthorization = function(id, data) {
			$scope.user = id;
			$http.defaults.headers.common.Authorization = data;
		}

		$scope.clearAuthorization = function() {
			$scope.user = '';
			$http.defaults.headers.common.Authorization = '';
		}

		$scope.clearLoginInfo = function() {
			$scope.email = '';
			$scope.password = '';
		}

		$scope.logout = function() {
			$scope.clearAuthorization();
			$location.url('/');
		}
	})

	.controller('SignupController', function($scope, $http) {

		$scope.email = '';
		$scope.password1 = '';
		$scope.password2 = '';

		$scope.signup = function() {

			if ($scope.password1 !== $scope.password2) {
				$scope.$parent.showError('Password does not match the confirm password.');
				$scope.password1 = '';
				$scope.password2 = '';
				return;
			}

			$http({
				method: 'put',
				url: 'api/users',
				data: {email: $scope.email, password: $scope.password1},
				headers: { "Content-Type": "application/json" }
			})
			.success(function(data, status, headers, config) {
				$scope.$parent.email = $scope.email;
				$scope.$parent.showSuccess(data, '/');
			})
			.error(function(data, status, headers, config) {
				$scope.$parent.showError(data);
			});
		}
	})

	.controller('ExpenseController', function($scope, $http) {

		$scope.date = new Date();

		$scope.add = function() {
			$http({
				method: 'put',
				url: 'api/expenses',
				data: {amount: $scope.amount, date: $scope.date.toISOString(), note: $scope.note},
				headers: { "Content-Type": "application/json" }
			})
			.success(function(data, status, headers, config) {
				$scope.$parent.showSuccess(data);
				$scope.date = new Date();
				$scope.amount = null;
				$scope.note = "";
			})
			.error(function(data, status, headers, config) {
				if (401 === status) {
					$scope.$parent.clearAuthorization();
				}
				$scope.$parent.showError(data, '/');
			});
		}
	})

	.controller('ReportController', function($scope, $http) {

		$scope.expenses = [];

		$scope.$parent.showWarning("Loading...");

		$http.get('api/expenses/report')
		.success(function(data, status, headers, config) {

			$scope.$parent.showWarning("");
			
			if (data.length < 1) {
				$scope.$parent.showError("no data");
			}
			else {
				$scope.expenses = data;
			}
		})
		.error(function(data, status, headers, config) {
			if (401 === status) {
				$scope.$parent.clearAuthorization();
			}
			$scope.$parent.showWarning("");
			$scope.$parent.showError(data, '/');
		});
	})

	.controller('GraphController', function($scope, $http) {

		$scope.labels = [];
		$scope.data = [[]];

		$scope.$parent.showWarning("Loading...");

		$http.get('api/expenses/graph')
		.success(function(data, status, headers, config) {

			$scope.$parent.showWarning("");

			if (data[0].length < 1) {
				$scope.$parent.showError("no data");
			}
			else {
				angular.forEach(data, function(v, k){
					$scope.labels.push(v[0]);
					$scope.data[0].push(v[1]);
				});
			}
		})
		.error(function(data, status, headers, config) {
			if (401 === status) {
				$scope.$parent.clearAuthorization();
			}
			$scope.$parent.showWarning("");
			$scope.$parent.showError(data, '/');
		});
	})

	.config(function($routeProvider, $locationProvider) {
		
		$routeProvider
			.when('/', {
				templateUrl: 'main.html'
			})
			.when('/signup', {
				templateUrl: 'signup.html',
				controller: 'SignupController'
			})
			.when('/expense', {
				templateUrl: 'expense.html',
				controller: 'ExpenseController'
			})
			.when('/report', {
				templateUrl: 'report.html',
				controller: 'ReportController'
			})
			.when('/graph', {
				templateUrl: 'graph.html',
				controller: 'GraphController'
			});

		$locationProvider.html5Mode({
			enabled: true,
			requireBase: false
		});
	});
})();