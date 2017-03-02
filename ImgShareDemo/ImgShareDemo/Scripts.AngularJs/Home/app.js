(function () {
    "use strict";
    angular.module("ISD.Services", ["ngResource", "ngFileUpload"]);
    angular.module("ISD.Controllers", ["ngFileUpload"]);
    angular.module("ISD.Directives", ["ui.bootstrap", "ngFileUpload", "ngSanitize"]);

    // Configure client side middle ware for all API requests
    angular.module("ISD.Services")
    .factory("ISD.Services.HttpInterceptor", Interceptor);

    angular.module("ISD.App", ["ISD.Services", "ISD.Controllers", "ISD.Directives"])
        .config(function ($httpProvider) {
            $httpProvider.interceptors.push("ISD.Services.HttpInterceptor");
        });



    Interceptor.$inject = ["$q"];

    function Interceptor($q) {
        return {
            request: _request,
            requestError: _requestError,
            response: _response,
            responseError: _responseError
        };

        function _request(config) {
            // success action
            return config;
        }

        function _requestError(rejection) {
            // error action
            return $q.reject(rejection);
        }

        function _response(fullResponse) {
            // response action
            return fullResponse;
        }

        function _responseError(response) {
            if (response.status && response.status === 401) // Unauthorized
            {
                $window.location = "/Account/Login";
            }
            return $q.reject(response);
        }
    }
})();