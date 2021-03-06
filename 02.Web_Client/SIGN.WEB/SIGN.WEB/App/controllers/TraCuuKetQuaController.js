﻿"use strict";

app.controller('TraCuuKetQuaController', function ($scope, $rootScope, $sce, $windo0w, TraCuuKetQuaService, fileReader) {
    // Initial object
    $scope.doc = {};
    $scope.newDoc = {};
    $scope.chungtuList = [];

    // Search documents
    $scope.searchDoc = function () {
        // start and limit fixed: 1 - 1000

        var promisePost = TraCuuKetQuaService.searchDocs($scope.doc, 1, 1000);
        promisePost.then(
            function (result) {
                $scope.chungtuList = result.data.Data;
            }, function (error) {
                console.log(error);
            });
    }
    $scope.searchDoc();

    // Get file
    $scope.file = null;
    $scope.getFile = function (file, index) {
        $scope.file = file;
        fileReader.readAsDataUrl($scope.file, $scope)
                      .then(function (result) {
                          // Get url source
                      });
    };

    //// Create new doc
    //$scope.addNewDoc = function () {
    //    var promisePost = EdataService.addNewDoc($scope.newDoc, $scope.file);
    //    promisePost.then(
    //        function (result) {

    //        }, function (error) {
    //            console.log(error);
    //        });
    //}
});