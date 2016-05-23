'use strict';
app.controller('loginController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.loginData = {
        userName: "",
        password: ""
    };
    $scope.departmentList = [];
    authService.getDepartments().then(function (response) {
        $scope.departmentList = response.data;
        $scope.departmentList.push({ id: 0, departmentName: "admin", departmentCode: 0 });
        $scope.departmentList.push({ id: 100, departmentName: "admin2", departmentCode: 0 });
    });
    $scope.message = "";
    $scope.login = function () {
        authService.login($scope.loginData).then(function (response) {
            $location.path('/values');
        },
         function (err) {
             $scope.message = err.error_description;
         });
    };

}]);