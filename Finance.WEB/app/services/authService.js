'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService','$location', function ($http, $q, localStorageService,$location) {

    var serviceBase = 'http://192.168.77.151/finance/';
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: ""
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });

    };

    var _login = function (loginData) {

        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: response.user_name, departmentCode: response.departmentCode });
            //console.log(localStorageService.get('authorizationData'));
            _authentication.isAuth = true;
            _authentication.userName = response.user_name;
            _authentication.departmentCode = response.departmentCode;
            deferred.resolve(response);

          //  console.log(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });
       // console.log(deferred.promise);
        return deferred.promise;
    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.departmentCode = "";
       // $location.path('/login')
       //console.log($location.path());

    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.departmentCode = authData.departmentCode;
        }

    }

    var _getDepartments = function () {
        return $http.get(serviceBase + "/api/department").then(function (results) {
            return results;
        })
    }

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.getDepartments = _getDepartments;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
   // console.dir(_authentication);

    return authServiceFactory;
}]);