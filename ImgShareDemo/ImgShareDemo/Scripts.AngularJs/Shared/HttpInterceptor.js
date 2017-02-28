(function () {
    "use strict";
    // Configure client side middle ware for all API requests
    angular.module("ISD.Services")
    .factory("ISD.Services.HttpInterceptor", Interceptor);

    Interceptor.$inject = ["$q"];

    function Interceptor($q)
    {
        return {
            request : _request,
            requestError : _requestError,
            response : _response,
            responseError : _responseError
        };
        
        function _request (config) {
            // success action
            return config;
        }

        function _requestError(rejection) {
            // error action
            return $q.reject(rejection);
        }

        function _response (fullResponse) {
            // response action
            return fullResponse;
        }

        function _responseError (response) {
            if (response.status && response.status === 401) // Unauthorized
            {
                $window.location = "/Account/Login";
            }
            return $q.reject(response);
        }
    }
});