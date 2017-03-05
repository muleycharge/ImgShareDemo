(function () {
    "use strict";

    angular
        .module("ISD.Services")
        .factory("ISD.Services.AssetService", Service)
        .value("AppConstants", AppConstants);

    Service.$inject = ["$http", "$log", "$q", "$resource", "AppConstants", "Upload"];

    function Service($http, $log, $q, $resource, appConstants, upload) {

        var service = {
            GetAssets: _getAssets,
            GetAsset: _getAsset,
            CreateAsset: _createAsset,
            DeleteAsset: _deleteAsset,
            UpdateAsset: _updateAsset,
            UploadAsset: _uploadToAsset,
            AddAssetTag: _addAssetTag,
            RemoveAssetTag : _removeAssetTag
        };

        var assetResource = $resource("/api/Asset/:assetId",
            { assetId: "@id" },
            {
                "update": { method: "PUT" },
                "upload": { method: "POST", url: "/api/Asset/Upload" },
                "addtag": { method: "POST", url: "/api/Asset/Tag/:id" },
                "removetag": { method: "POST", url: "/api/Asset/RemoveTag/:id" }
            });

        return service;


        function _getAssets(search, take, offset){
            return assetResource.get({ search: search, take: take, offset: offset }).$promise;
        }

        function _getAsset(id) {
            return assetResource.get({ id: id }).$promise;
        }

        function _createAsset(asset){
            asset.UserId = AppConstants.UserId;
            return assetResource.save({}, asset).$promise;
        }

        function _deleteAsset(id) {
            return assetResource.delete({ id: id }).$promise;
        }

        function _updateAsset(asset){
            asset.UserId = AppConstants.UserId;
            return assetResource.update({}, asset).$promise;
        }

        function _uploadToAsset(asset, file, progressCallback){
            var deferred = $q.defer();
            var progress = progressCallback || angular.noop;
            upload.http({
                url: "/api/Asset/Upload/" + asset.Id,
                data: file,
                headers: {
                    'Content-Type': file.type
                }
            }).then(deferred.resolve, deferred.reject, progress);

            return deferred.promise;
        }

        function _addAssetTag(assetId, tag){
            var promise = assetResource.addtag({ id: assetId }, tag).$promise;
            return promise;
        }

        function _removeAssetTag(assetId, tag) {
            var promise = assetResource.removetag({ id: assetId }, tag).$promise;
            return promise;

        }
    }
})();