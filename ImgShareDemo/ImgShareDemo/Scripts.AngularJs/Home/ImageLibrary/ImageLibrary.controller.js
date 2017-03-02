(function () {
    "use strict";

    angular
        .module("ISD.Controllers")
        .controller("ISD.Controllers.ImageLibrary", ImageLibrary);

    ImageLibrary.$inject = ["$location", "$log", "ISD.Services.AssetService"];

    function ImageLibrary($location, $log, assetService) {
        /* jshint validthis:true */
        var vm = this;

        activate();

        function activate()
        {
            assetService.GetAssets("", 100, 0)
            .then(function (result) {
                vm.Assets = result.Data.Items;
            })
        }
    }
})();
