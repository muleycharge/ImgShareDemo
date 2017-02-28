(function () {
    "use strict";
    
    angular.module("ISD.Services", ["ngResource"])
    angular.module("ISD.Controllers", [])
    angular.module("ISD.Directives", ["ui.bootstrap"])

    angular.module("ISD.App", ["ISD.Services", "ISD.Controllers", "ISD.Directives"])
        .config(function ($httpProvider) {
            $httpProvider.interceptors.push("ISD.Services.HttpInterceptor");
        });
})();