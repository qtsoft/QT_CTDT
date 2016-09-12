"use strict";

app.controller('ChungTuController', function ($scope, $rootScope, $sce, $window, CommmonService) {
    $scope.statusList = ['Chưa gửi', 'Đã gửi'];
    $scope.typeList = ['Phiếu xuất', 'Phiếu kho'];
    $scope.btnList = [
            //{ name: 'add', value: 'Thêm' },
            { name: 'search', value: 'Tìm kiếm' }
    ];
    $scope.chungtu = {
        type: $scope.typeList[0],
        code: '1111111',
        status: $scope.statusList[0],
        unit: 'Tập đoàn viễn thông quân đội',
        name: 'chung tu abc'
    };
    CommmonService.call('user').then(function (dataResponse) {
        
    }, function (error) {
        console.log('wrong call');
    });
    $scope.search = function () {
        console.log($scope.chungtu.type);
    }
    function search() {

    }
});