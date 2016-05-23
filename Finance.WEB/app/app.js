var app = angular.module('FinanceApp', ['ngRoute', 'LocalStorageModule', 'ngMaterial', 'angular-loading-bar']);

app.config(function ($routeProvider) {
       

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "app/views/signup.html"
    });

    $routeProvider.when("/values", {
        controller: "valuesController",
        templateUrl: "app/views/values.html"
    });

    $routeProvider.when("/root", {
        controller: "rootController",
        templateUrl: "app/views/root.html"
    });

    $routeProvider.when("/admin", {
        controller: "adminController",
        templateUrl: "app/views/admin.html"
    });

    $routeProvider.otherwise({ redirectTo: "/values" });
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);
app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});