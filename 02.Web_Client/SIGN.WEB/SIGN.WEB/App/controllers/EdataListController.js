"use strict";

app.controller('EdataListController', function ($scope, $rootScope, $sce, $window, EdataService) {
    // Initial object
    $scope.doc = {};


    // Search documents
    $scope.searchDoc = function() {
        var promisePost = EdataService.searchDocs($scope.doc);
        promisePost.then(
            function (result) {
                console.log("success!");
            }, function (error) {
                //...
            });
    }
});