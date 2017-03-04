(function () {
    "use strict";

    angular
        .module("ISD.Services")
        .factory("ISD.Services.TagService", Service)
        .value("AppConstants", AppConstants);

    Service.$inject = ["$http", "$resource", "AppConstants"];

    function Service($http, $resource, appConstants) {

        var service = {
            GetTags: _getTags,
            CreateTag: _createTag,
            DeleteTag: _deleteTag,
        };

        var tagResource = $resource("/api/Tag/:tagId",
            { tagId: "@id" },
            {
                "update": { method: "PUT" },
                "upload": { method: "POST", url: "/api/Tag/Upload" }
            });

        return service;


        function _getTags(search, take, offset) {
            return tagResource.get({ search: search, take: take, offset: offset }).$promise;
        }

        function _createTag(name) {
            // Seems the only way to pass this to the body thanks to:
            // http://jasonwatmore.com/post/2014/04/18/post-a-simple-string-value-from-angularjs-to-net-web-api
            return tagResource.save({}, '"' + name + '"').$promise;
        }

        function _deleteTag(id) {
            return tagResource.delete({ id: id }).$promise;
        }
    }
})();