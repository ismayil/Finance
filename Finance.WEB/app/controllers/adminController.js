'use strict';
app.controller('adminController', ['$scope', '$location', 'authService', 'adminService', function ($scope, $location, authService, adminService) {
    var auth = authService.authentication;
    if (!auth.isAuth || auth.departmentCode != 0) {
        authService.logOut();
        $location.path('/login');
    }
}]);
app.controller('adminUserController', ['$mdDialog','adminService', 'valuesService', function ($mdDialog,adminService,valueService) {
    var parent = this;
    parent.departmentList = [];
    parent.userList = [];

    valueService.getDepartments().then(function (results) {
        parent.departmentList = results.data;
       // console.log(parent.departmentList);
    });

    adminService.getUsers().then(function (results) {
        parent.userList = results.data;       // console.log(parent.userList);
    });
    parent.showConfirm = function (ev, id, index) {
        var confirm = $mdDialog.confirm()
              .title('Bu istifadəçi silmək istədiyinizdən əminsiniz?')              
              .targetEvent(ev)
              .ok('Bəli, silinsin!')
              .cancel('Xeyr, hələlik saxla');
        $mdDialog.show(confirm).then(function () {
            if (adminService.deleteUser(id)) {
                parent.userList.splice(index, 1);
            }
        }, function () {
            // $scope.status = 'You decided to keep your debt.';
        });
    };
   // this.thisUser = {};
    this.editUser = function (user) {
        adminService.updateUser(user.id, { userName: user.userName, DepartmentCode: user.department.departmentCode }).then(function () {
            parent.userList[user.index] = { userName: user.userName, departmentName: user.department.departmentName };
        });
       // console.log(user);
    }
}]);
app.controller('adminDepartmentController', ['$mdDialog', 'adminService', 'valuesService', function ($mdDialog, adminService, valueService) {
    var parent = this;
    parent.departmentList = [];
    parent.userList = [];

    valueService.getDepartments().then(function (results) {
        parent.departmentList = results.data;
       // console.log(parent.departmentList);
    });
   
    parent.showConfirm = function (ev, id, index) {
        var confirm = $mdDialog.confirm()
              .title('Bu istifadəçi silmək istədiyinizdən əminsiniz?')
              .targetEvent(ev)
              .ok('Bəli, silinsin!')
              .cancel('Xeyr, hələlik saxla');
        $mdDialog.show(confirm).then(function () {
            if (adminService.deleteDepartment(id)) {
                parent.departmentList.splice(index, 1);
            }
        }, function () {
            // $scope.status = 'You decided to keep your debt.';
        });
    };
     parent.thisDepartment = {};
    this.editDepartment = function (department) {
        adminService.updateDepartment(department).then(function () {
            parent.thisDepartment = {};
            if (!angular.isNumber(department.index)) {
                parent.departmentList.push(department);
            }
        });
    }
}]);
app.controller('adminDateController', ['$mdDialog', 'adminService', 'valuesService', function ($mdDialog, adminService, valueService) {
    var parent = this;
    parent.dateList = [];
    valueService.getDates().then(function (results) {
        parent.dateList = results.data;
        //console.log(parent.dateList);
    });

    parent.showConfirm = function (ev, id, index) {
        var confirm = $mdDialog.confirm()
              .title('Bu istifadəçi silmək istədiyinizdən əminsiniz?')
              .targetEvent(ev)
              .ok('Bəli, silinsin!')
              .cancel('Xeyr, hələlik saxla');
        $mdDialog.show(confirm).then(function () {
            if (adminService.deleteDate(id)) {
                parent.dateList.splice(index, 1);
            }
        }, function () {
            // $scope.status = 'You decided to keep your debt.';
        });
    };
    parent.thisDate = {};
    this.editDate = function (date) {
        adminService.updateDate(date).then(function () {
            parent.thisDate = {};
            if (!angular.isNumber(date.index)) {
                parent.dateList.push(date);
            }
        });
    }
}]);
app.controller('adminTitleController', ['$mdDialog', 'adminService', 'valuesService', function ($mdDialog, adminService, valueService) {
    var parent = this;
    parent.titleList = [];
    adminService.getTitles().then(function (results) {
        parent.titleList = results.data;
       // console.log(parent.titleList);
    });

    parent.showConfirm = function (ev, id, index) {
        var confirm = $mdDialog.confirm()
              .title('Bu istifadəçi silmək istədiyinizdən əminsiniz?')
              .targetEvent(ev)
              .ok('Bəli, silinsin!')
              .cancel('Xeyr, hələlik saxla');
        $mdDialog.show(confirm).then(function () {
            if (adminService.deleteTitle(id)) {
                parent.titleList.splice(index, 1);
            }
        }, function () {
            // $scope.status = 'You decided to keep your debt.';
        });
    };
    parent.thisTitle = {};
    this.editTitle = function (title) {
        adminService.updateTitle(title).then(function () {
            parent.thisTitle = {};
            if (!angular.isNumber(title.index)) {
                parent.titleList.push(title);
            }
        });
    }
}]);
app.controller('adminLockController', ['$mdDialog', 'adminService', function ($mdDialog, adminService) {
    var parent = this;
    parent.lockList = [];
    adminService.getLocks().then(function (results) {
        parent.lockList = results.data;
      //  console.log(parent.titleList);
    });
    parent.updateLock = function (lock, index) {
        adminService.updateLock(lock).then(function () {
            if (lock.localStatus == false) parent.lockList.splice(index, 1);
        });
    }
    //parent.showConfirm = function (ev, id, index) {
    //    var confirm = $mdDialog.confirm()
    //          .title('Bu istifadəçi silmək istədiyinizdən əminsiniz?')
    //          .targetEvent(ev)
    //          .ok('Bəli, silinsin!')
    //          .cancel('Xeyr, hələlik saxla');
    //    $mdDialog.show(confirm).then(function () {
    //        if (adminService.deleteTitle(id)) {
    //            parent.titleList.splice(index, 1);
    //        }
    //    }, function () {
    //        // $scope.status = 'You decided to keep your debt.';
    //    });
    //};
    parent.thisTitle = {};
    this.editTitle = function (title) {
        adminService.updateTitle(title).then(function () {
            parent.thisTitle = {};
            if (!angular.isNumber(title.index)) {
                parent.titleList.push(title);
            }
        });
    }
}]);