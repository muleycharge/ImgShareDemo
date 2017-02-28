(function () {
    "use strict";

    angular
        .module("app")
        .controller("ISD.Controllers.ImageLibrary", ImageLibrary);

    ImageLibrary.$inject = ["$location"]; 

    function ImageLibrary($location) {
        /* jshint validthis:true */
        var vm = this;

        activate();

        function activate() { }
    }
})();
