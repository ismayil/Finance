'use strict';
app.controller('valuesController', ['$scope', '$location', 'valuesService', 'authService', '$mdDialog', function ($scope, $location, valuesService, authService, $mdDialog) {
    var auth = authService.authentication;
    $scope.departmentList = [];
    $scope.departmentCode = auth.departmentCode;
    $scope.authDepartmentCode = auth.departmentCode;
    $scope.rowTotalLocalDebit = 0;
    $scope.updateRowTotalLocalDebit = function (val, type) {
        if (type == true) $scope.rowTotalLocalDebit += val;
        else $scope.rowTotalLocalDebit -= val;
    };
    if (auth.departmentCode == 0) {
        valuesService.getDepartments().then(function (results) {
            $scope.departmentList = results.data;
            $location.path('/root');
        })
    };
    $scope.localLockStatus = true;
    $scope.remoteLockStatus = true;
    $scope.valuesLi = 0;
    $scope.valuesSetLi = function (li, dateId) {
        //console.log(dateId);
        $scope.valuesLi = li;
        $scope.dateId = dateId;
        var data = { DateId: dateId, DepartmentCode: $scope.departmentCode };
        reloadData(data);
    };
    $scope.valuesDepartment = 0;
    $scope.valuesSetDepartment = function (li, departmentId) {
        $scope.valuesDepartment = li;
        $scope.departmentCode = departmentId;
        var data = { DateId: $scope.dateId, DepartmentCode: departmentId };
        //  console.log(data);
        reloadData(data);
    };
    var reloadData = function (data) {
        valuesService.getValues(data).then(function (results) {
            $scope.debits = results.data.debit;
            $scope.kredits = results.data.kredit;
            $scope.localLockStatus = results.data.lockStatus.local;
            $scope.remoteLockStatus = results.data.lockStatus.remote;
            // console.log(results);
        }, function (error) {
            //alert(error.data.message);
        });
    }
    $scope.valuesIsSetLi = function (li) {
        return ($scope.valuesLi === li);
    }
    $scope.valuesIsSetDepartment = function (li) {
        return ($scope.valuesDepartment === li);
    }
    $scope.debits = [];
    $scope.kredits = [];

    $scope.departmentDates = [];
    valuesService.getDates().then(function (results) {
        $scope.departmentDates = results.data;
        // console.log(results);
    }, function (error) {
        //alert(error.data.message);
    });

    $scope.titles = [];
    valuesService.getTitles().then(function (results) {
        $scope.titles = results.data;
        //console.log(results);
    }, function (error) {
        //alert(error.data.message);
    });
    $scope.showConfirm = function (ev, id, index, types) {
        //console.dir(ev);
        // Appending dialog to document.body to cover sidenav in docs app
        var confirm = $mdDialog.confirm()
              .title('Bu məlumatı silmək istədiyinizdən əminsiniz?')
              //.textContent('All of the banks have agreed to forgive you your debts.')
              //.ariaLabel('Lucky day')
              .targetEvent(ev)
              .ok('Bəli, silinsin!')
              .cancel('Xeyr, hələlik saxla');
        $mdDialog.show(confirm).then(function () {
            if (valuesService.deleteValues(id)) {
                $scope[types].splice(index, 1);
               // $scope.updateRowTotalLocalDebit(newData.value, false);
            }
        }, function () {
            // $scope.status = 'You decided to keep your debt.';
        });
    };
    $scope.totalRow = function (type,dateId) {
        var total = {};
        total[dateId] = 0;
        angular.forEach($scope[type], function (item) {
            total[dateId] += item.value;
           // console.log(total[dateId]);
        })

        return total[dateId];
    };
}]);
app.controller('newValuesController', ['$scope', 'valuesService', function ($scope, valuesService) {
    var DebKre = { 'debits': 250, 'kredits': 630 };

    this.addDebitKredit = function (debit, types) {
        this.newDebit = { DateId: $scope.dateId, DepartmentCode: $scope.departmentCode, ValueList: [] };
        this.newDebit.DebitKredit = DebKre[types];
        var newData = { titlecode: debit.title.id, title: debit.title.title, description: debit.title.description, value: debit.value };
        this.newDebit.ValueList.push(newData);
        console.dir(this.newDebit);
        if (valuesService.postValues(this.newDebit)) {
            $scope[types].push(newData);
           // $scope.updateRowTotalLocalDebit(newData.value, true);
        }

    }
    this.saveLocalAll = function () {
        var data = { departmentCode: $scope.departmentCode, dateId: $scope.dateId, localStatus: true };
        if (valuesService.postLocks(data)) {
            $scope.localLockStatus = true;
        }
    };
    this.saveRemoteAll = function () {
        var data = { departmentCode: $scope.departmentCode, dateId: $scope.dateId, remoteStatus: true, localStatus: true };
        if (valuesService.postLocks(data)) {
            $scope.remoteLockStatus = true;
        }
    };
    
}]);
