'use strict';
app.factory('valuesService', ['$http','$q', function ($http,$q) {

    var serviceBase = 'http://192.168.77.151/finance/';
    var valuesServiceFactory = {};

    var _getValues = function (data) {       
        return $http.post(serviceBase + 'api/value/get',data).then(function (results) {
            return results;
        });
    };
    var _getDates = function () {       
        return $http.get(serviceBase + 'api/date').then(function (results) {
            return results;
        });
    };
    var _getTitles = function () {
        return $http.get(serviceBase + 'api/title').then(function (results) {
            return results;
        });
    };
    var _getDepartments = function () {
        return $http.get(serviceBase + 'api/department').then(function (results) {
            return results;
        });
    };
    var _deleteValues = function (id) {
        return $http.delete(serviceBase + 'api/value/' + id).then(function (results) {
          //  console.log(results);
            return results;
        });
    };
    var _postValues = function (data) {
        var deferred = $q.defer();
        return $http.post(serviceBase + 'api/value', data).success(function (response) {
            deferred.resolve(response);
        }).error(function (err,status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };
    var _getLocks = function (data) {
        return $http.post(serviceBase + 'api/lock/current', data).then(function (results) {
            return results;
        });
    };

    var _postLocks= function (data) {
        return $http.post(serviceBase + 'api/lock', data).then(function (results) {
            return results;
        });
    };
    valuesServiceFactory.getValues = _getValues;
    valuesServiceFactory.getDates = _getDates;
    valuesServiceFactory.getTitles = _getTitles;
    valuesServiceFactory.postValues = _postValues;
    valuesServiceFactory.deleteValues = _deleteValues;
    valuesServiceFactory.getLocks = _getLocks;
    valuesServiceFactory.postLocks = _postLocks;
    valuesServiceFactory.getDepartments = _getDepartments;
    return valuesServiceFactory;

}]);