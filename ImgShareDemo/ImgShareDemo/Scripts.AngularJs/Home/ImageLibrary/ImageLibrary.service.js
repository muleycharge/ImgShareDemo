(function () {
    'use strict';

    angular
        .module('app')
        .factory('ISD.Services.ImageLibrary', ImageLibrary);

    ImageLibrary.$inject = ['$http'];

    function ImageLibrary($http) {
        var service = {
            getData: getData
        };

        return service;

        function getData() { }
    }
})();