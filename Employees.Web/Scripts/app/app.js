angular.module('app', ['ngRoute', 'restangular', 'ui.bootstrap', 'app.employee'])

    .constant('ApiBaseUrl', '/api')

    .config(['$routeProvider', '$locationProvider', 'RestangularProvider', 'ApiBaseUrl',
        function ($routeProvider, $locationProvider, restangularProvider, apiBaseUrl) {

            $routeProvider.when('/', { redirectTo: '/employees' });

            $routeProvider.when('/employee',
                {
                    reloadOnSearch: false,
                    controller: 'EmployeeController',
                    templateUrl: '/scripts/app/employee/employee.html',
                    resolve: {
                        employee: ['Restangular', '$route', function (restangular, $route) {
                            var id = $route.current.params.id;

                            if (id) {
                                return restangular.one('employees', id).get();
                            } else {
                                return {
                                    Id: null,
                                    Name: "",
                                    Email: "",
                                    post: function () {
                                        return restangular.all('employees').post(this);
                                    }
                                };
                            }
                        }]
                    }
                });

            $routeProvider.when('/employees',
                {
                    controller: 'EmployeeListController',
                    templateUrl: '/scripts/app/employee/employee-list.html',
                    resolve: {
                        employees: ['Restangular', function (restangular) {
                            return restangular.all('employees').getList();
                        }]
                    }
                });

            $locationProvider.html5Mode(true);

            restangularProvider.setBaseUrl(apiBaseUrl);
            restangularProvider.setRestangularFields({ id: "Id" });
        }]);
