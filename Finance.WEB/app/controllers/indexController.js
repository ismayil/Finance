'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {
    var auth = authService.authentication;
    //if (!auth.isAuth && $location.path() != '/signup') $location.path('/login');
    //console.log(auth.isAuth);
    //console.log($location.path());
    $scope.logOut = function () {
        authService.logOut();
        $location.path('/login');
    }
    $scope.authentication = auth;

}]);