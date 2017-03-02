/// <reference path="ImageList.directive.html" />
/// <reference path="ImageList.directive.html" />
(function() {
    "use strict";

    angular
        .module("ISD.Directives")
        .directive("imageList", ImageList);

    ImageList.$inject = ["$window", "ISD.Services.AssetService"];
    
    function ImageList ($window, assetService) {
        // Usage:
        //     <image-list></image-list>
        // Creates:
        // 
        var directive = {
            templateUrl: "/Scripts.AngularJs/Home/ImageLibrary/ImageList.directive.html",
            link: link,
            controller: controller,
            bindToController: true,
            controllerAs: "imageListCtrl",
            restrict: "A",
            scope: {
                Assets: "=assets",
                _loadMore: "&loadMore"
            }
        };
        return directive;

        function link(scope, element, attrs) {
        }

        function controller($scope) {
            var vm = this;
            vm.waiting = false;
            vm.search = "";
            vm.Assets = vm.Assets || [];
            vm.Delete = _delete;
            vm.Update = _update;
            vm.Upload = _upload


            function _delete(Asset) {
                return assetService.DeleteAsset(Asset);
            }

            function _update(Asset) {
                return assetService.UpdateAsset(Asset);
            }

            function _upload(asset, $file, uploadStatusCallback) {
                var promise = assetService.UploadAsset(asset, $file, uploadStatusCallback);
                return promise;
            }
        }
    }

})();