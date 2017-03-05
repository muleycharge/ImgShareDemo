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
                _loadMore: "&loadMore",
                draggedTag: "="
            }
        };
        return directive;

        function link(scope, element, attrs) {
        }

        function controller($scope) {
            var vm = this;
            vm.waiting = false;
            vm.search = "";
            vm.take = 10;
            vm.offset = 0;
            vm.lastPage = false;

            vm.Assets = vm.Assets || [];
            vm.Delete = _delete;
            vm.Update = _update;
            vm.Upload = _upload
            vm.Search = _search;
            vm.AddImg = _addImage;
            vm.AddTag = _addTag;
            vm.RemoveTag = _removeTag;

            _init();

            function _init() {
                _search();
                $scope.$watch("imageListCtrl.search", function (newVal, oldVal) {
                    if (newVal !== oldVal) {
                        _search();
                    }
                });
            }
            // Used for tracking new image since they don't have IDs
            var _newCount = 1;
            function _addImage() {
                vm.Assets.unshift({
                    Name: "",
                    Description: "",
                    SourceUrl: "",
                    $vm_id: _newCount,
                    Tags: []
                });
                _newCount++;
            }

            function _delete(Asset) {
                var i = vm.Assets.indexOf(Asset);
                if (!Asset.Id && Asset.$vm_id) {
                    _.remove(vm.Assets, { $vm_id: Asset.$vm_id })
                }
                else if (Asset.Id)
                {
                    return assetService.DeleteAsset(Asset.Id)
                        .then(function(response){
                            _.remove(vm.Assets, { Id : Asset.Id })
                        });
                }
            }

            function _update(Asset) {
                var promise = null;
                if (Asset.Id)
                {
                    promise = assetService.UpdateAsset(Asset);
                }
                else {
                    promise = assetService.CreateAsset(Asset)
                }
                return promise;
            }

            function _upload(asset, $file, uploadStatusCallback) {
                var promise = assetService.UploadAsset(asset, $file, uploadStatusCallback);
                return promise;
            }

            function _addTag(assetId, tag)
            {
                var promise = assetService.AddAssetTag(assetId, tag);
                return promise;
            }

            function _removeTag(assetId, tag)
            {
                var promise = assetService.RemoveAssetTag(assetId, tag);
                return promise;
            }

            function _search()
            {
                var promise = assetService.GetAssets(vm.search, vm.take, 0)
                    .then(function(result){
                        if (_.has(result, "Data.Items")) {
                            vm.Assets = result.Data.Items;
                            vm.lastPage = result.Data.LastPage;
                            vm.offset = result.Data.Items.length;
                        }
                    });
                return promise;
            }
        }
    }

})();