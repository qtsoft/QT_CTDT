﻿"use strict";
var service = angular.module("Service", []);
var directive = angular.module("Directive", []);
var app = angular.module("ClientApp", ["ngRoute", "Service","Directive"]);

// Show Routing.
app.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/",
        {
            caseInsensitiveMatch: true,
            templateUrl: "Home/ChungTu",
            title: "Chung tu",
            controller: "ChungTuController"
        });
    $routeProvider.when("/chung-tu",
        {
            caseInsensitiveMatch: true,
            templateUrl: "Home/ChungTu",
            title: "Chung tu",
            controller: "ChungTuController"
        });
    $routeProvider.when("/notfound",
        {
            caseInsensitiveMatch: true,
            title: 'Not found',
            templateUrl: "/Client/NotFound"
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
        $rootScope.apiURL = "http://localhost:58474";

        // Base Url of web app.
        $rootScope.BaseUrl = angular.element($('#BaseUrl')).val();
    }]);
