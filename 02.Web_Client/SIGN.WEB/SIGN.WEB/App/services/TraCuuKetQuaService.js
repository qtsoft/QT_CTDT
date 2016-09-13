"use strict";

service.service("TraCuuKetQuaService", function ($http, $rootScope, $window, $location) {

    // Function to search docs
    this.searchDocs = function (data, start, limit) {
        var request = $http({
            method: 'get',
            url: $rootScope.apiURL + '/api/ChungTuApi/GetByFilter',
            params: {
                key: data.Name,
                maChungTu: data.Id,
                maLoaiChungTu: data.type,
                donViBanHanh: data.Unit,
                trangThai: data.Status,
                start: start,
                limit: limit
            }
        });
        return request;
    }

});