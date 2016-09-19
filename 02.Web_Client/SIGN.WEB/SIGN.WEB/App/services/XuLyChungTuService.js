"use strict";

service.service("XuLyChungTuService", function ($http, $rootScope, $window, $location) {

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

    this.addNewDoc = function (data, file) {
        var fdata = new FormData();
        var url = $rootScope.apiURL + '/api/ChungTuApi/Create';

        fdata.append('file', file);
        fdata.append('data', JSON.stringify(data));

        return $http.post(url, fdata, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    }

    this.processDoc = function (data) {
        var fdata = new FormData();
        var url = $rootScope.apiURL + '/api/ChungTuApi/Signature';

        //fdata.append('file', file);
        fdata.append('data', JSON.stringify(data));

        return $http.post(url, fdata, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    }


});