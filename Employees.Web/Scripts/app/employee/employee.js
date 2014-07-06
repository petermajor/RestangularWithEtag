angular.module('app.employee', [])

    .controller('EmployeeListController', ['$scope', '$location', 'employees',
        function ($scope, $location, employees) {
            $scope.employees = employees;

            $scope.onAdd = function() {
                $location.url('/employee');
            };
        }])

    .controller('EmployeeController', ['$scope', '$location', '$modal', 'employee',
        function ($scope, $location, $modal, employee) {
            $scope.employee = employee;

            $scope.onSave = function () {
                $scope
                    .postOrPut()
                    .then($scope.saveSuccess)
                    .catch($scope.saveFailure);
            };

            $scope.postOrPut = function() {
                if ($scope.employee.Id) {
                    return $scope.employee.put();
                } else {
                    return $scope.employee.post();
                }
            }

            $scope.saveSuccess = function(updatedEmployed) {
                $scope.employee = updatedEmployed;
                $scope.lastSave = new Date();

                if (!$location.search.id) {
                    var search = $location.search();
                    search.id = $scope.employee.Id;
                    $location.search(search).replace();
                }
            }

            $scope.saveFailure = function (reason) {
                if (reason.status === 412) {
                    $scope.showConcurrencyError();
                } else {
                    alert("failure - status: " + reason.status);
                };
            }

            $scope.showConcurrencyError = function() {
                $modal.open({
                    templateUrl: '/scripts/app/employee/concurrency.html',
                    size: 'sm'
                });
            };

            $scope.onCancel = function() {
                $location.url('/employees');
            }
        }]);