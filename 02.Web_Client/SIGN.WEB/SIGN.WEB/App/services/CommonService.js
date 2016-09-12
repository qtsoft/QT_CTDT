"use strict";

service.service("CommmonService", function ($http, $window, $location) {

    // Function to ....
    this.call = function (newUser) {
        var request = $http({
            method: 'post',
            url: '/api/UserApi/Register',
            data: newUser
        });

        return request;
    }

});