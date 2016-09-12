"use strict";

service.service("EdataService", function ($http, $rootScope, $window, $location) {

    // Function to search docs
    this.searchDocs = function (data) {
        var request = $http({
            method: 'post',
            url: $rootScope.apiURL + '/api/ChungTuApi/GetByFilter',
            data: data
        });
        return request;
    }

});