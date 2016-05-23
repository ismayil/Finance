'use strict';
app.factory('adminService', ['$http', '$q', function ($http, $q) {

    var serviceBase = 'http://192.168.77.151/finance/';
    var adminServiceFactory = {};

    var _getUsers = function () {
        return $http.get(serviceBase + 'api/account').then(function (results) {
            return results;
        });
    };
    var _getTitles = function () {
        return $http.get(serviceBase + 'api/title').then(function (results) {
            return results;
        });
    };
    var _deleteUser = function (id) {
        return $http.delete(serviceBase + 'api/account/'+id).then(function (results) {
            return results;
        });
    };
    var _deleteDepartment = function (id) {
        return $http.delete(serviceBase + 'api/department/' + id).then(function (results) {
            return results;
        });
    };
    var _deleteDate = function (id) {
        return $http.delete(serviceBase + 'api/date/' + id).then(function (results) {
            return results;
        });
    };
    var _deleteTitle = function (id) {
        return $http.delete(serviceBase + 'api/title/' + id).then(function (results) {
            return results;
        });
    };
    var _updateUser = function (id,user) {
        return $http.put(serviceBase + 'api/account/' + id,user).then(function (results) {
            return results;
        });
    };
    var _updateDepartment = function (deparment) {
        return $http.post(serviceBase + 'api/department', deparment).then(function (results) {
            return results;
        });
    };
    var _updateDate = function (date) {
        return $http.post(serviceBase + 'api/date', date).then(function (results) {
            return results;
        });
    };
    var _updateTitle = function (title) {
        return $http.post(serviceBase + 'api/title', title).then(function (results) {
            return results;
        });
    };
    var _getLocks = function () {
        return $http.get(serviceBase + 'api/lock').then(function (results) {
            return results;
        });
    };
    var _updateLock = function (lock) {
        return $http.put(serviceBase + 'api/lock/'+lock.id, lock).then(function (results) {
            return results;
        });
    };
    adminServiceFactory.getUsers = _getUsers;
    adminServiceFactory.getTitles = _getTitles;
    adminServiceFactory.getLocks = _getLocks;
    adminServiceFactory.deleteUser = _deleteUser;
    adminServiceFactory.deleteDepartment = _deleteDepartment;
    adminServiceFactory.deleteDate = _deleteDate;
    adminServiceFactory.deleteTitle = _deleteTitle;
    adminServiceFactory.updateUser = _updateUser;
    adminServiceFactory.updateDepartment = _updateDepartment;
    adminServiceFactory.updateDate = _updateDate;
    adminServiceFactory.updateTitle = _updateTitle;
    adminServiceFactory.updateLock = _updateLock;
    return adminServiceFactory;
}]);