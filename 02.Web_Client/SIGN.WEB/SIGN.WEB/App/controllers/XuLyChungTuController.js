"use strict";

app.controller('XuLyChungTuController', function ($scope, $rootScope, $sce, $window, XuLyChungTuService, EdataService) {
    // Initial object
    $scope.doc = {};
    $scope.chungtuList = [];

    // Search documents
    $scope.searchDoc = function () {
        // start and limit fixed: 1 - 1000

        var promisePost = EdataService.searchDocs($scope.doc, 1, 1000);
        promisePost.then(
            function (result) {
                $scope.chungtuList = result.data.Data;
            }, function (error) {
                console.log(error);
            });
    }
    $scope.searchDoc();
});