"use strict";
var service = angular.module("Service", []);
var directive = angular.module("Directive", []);
var app = angular.module("ClientApp", ["ngRoute", "Service","Directive"]);

// Show Routing.
app.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/home",
        {
            caseInsensitiveMatch: true,
            redirectTo: "/"
        });
    $routeProvider.when("/",
        {
            caseInsensitiveMatch: true,
            templateUrl: "/Home/EdataList",
            controller: "EdataListController"
        });
    $routeProvider.otherwise({
        redirectTo: "/"
    });
}])

app.run(['$rootScope', '$window','$location','$route',
    function ($rootScope, $window,$location,$route) {
        $rootScope.$on('$routeChangeError', function (e, curr, prev) {
            e.preventDefault();
        });

        // API URL
        $rootScope.apiURL = "xxx";

        // Base Url of web app.
        $rootScope.BaseUrl = angular.element($('#BaseUrl')).val();
    }]);
