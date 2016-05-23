'use strict';
app.controller('rootController', ['$scope', '$location', 'authService', 'rootService', 'valuesService','$timeout','$filter', function ($scope, $location, authService, rootService, valuesService,$timeout,$filter) {
    var parent = this
    var auth = authService.authentication;
    $scope.dateList = [];
    $scope.departmentList = [];
    $scope.totalValues = [];
    if (!auth.isAuth || auth.departmentCode != 0) {
        authService.logOut();
        $location.path('/login');
    }
    $scope.valuesLi = 0;
    $scope.valuesSetLi = function (li, dateId) {
        //console.log(dateId);
        $scope.valuesLi = li;
        $scope.dateId = dateId;
        // var data = { DateId: dateId, DepartmentCode: $scope.departmentCode };
        var data = { dateid: $scope.dateId, DepartmentCode: $scope.departmentCode };
        loadData(data);
    };
    $scope.valuesIsSetLi = function (li) {
        return ($scope.valuesLi === li);
    };
    valuesService.getDates().then(function (results) {
        $scope.dateList = results.data;
        // console.log($scope.dateList);
    });
    valuesService.getTitles().then(function (results) {
        $scope.titleList = results.data;
        // console.log($scope.titleList);
    });
    var loadData = function (data) {
        rootService.getTotalValues(data).then(function (results) {
            //  console.log(results.data);
            $scope.totalValues = results.data;
        });
    };
    $scope.valuesDepartment = 0;
    $scope.valuesSetDepartment = function (li, departmentId) {
        $scope.valuesDepartment = li;
        $scope.departmentCode = departmentId;
        var data = { dateid: $scope.dateId, DepartmentCode: $scope.departmentCode };
        //  console.log(data);
        loadData(data);
    };
    valuesService.getDepartments().then(function (results) {
        $scope.departmentList = results.data;
        // console.log($scope.departmentList);
    })
    $scope.valuesIsSetDepartment = function (li) {
        return ($scope.valuesDepartment === li);
    };

    $scope.exportToExcel = function (tableId,sheetName) { // ex: '#my-table'
        $scope.exportHref = rootService.excelFactory.tableToExcel(tableId, sheetName);
        $timeout(function () { location.href = $scope.exportHref; }, 100); // trigger download
    };

    $scope.totalRow = function (type,root ,dateId) {
        var total = {};
        total[dateId] = {};
        total[dateId][root] = 0;
        angular.forEach($scope.totalValues[type], function (item) {
            total[dateId][root] += item[root];
            //console.log(total[dateId][root]);
        });

        return total[dateId][root];
    };
    $scope.rightTotal = function (type, department, dateId) {
        var total = {};
        total[dateId] = {};
        total[dateId][department] = 0;
        var departmentArray = $filter('filter')($scope.totalValues[type], { departmentName: department });
        angular.forEach(departmentArray, function (item) {
            angular.forEach(item, function (val, key) {               
                if (key.toLowerCase() != 'departmentname' && key.toLowerCase() != 'dates' && angular.isNumber(val)) {
                    total[dateId][department] += val;
                };                
            });
        });
        return total[dateId][department];
    };

}]);