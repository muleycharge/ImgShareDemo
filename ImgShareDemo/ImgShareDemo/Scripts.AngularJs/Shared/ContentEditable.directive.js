(function() {
    "use strict";

    angular
        .module("ISD.Directives")
        .directive("contenteditable", ContentEditable);

    ContentEditable.$inject = ["$sce"];
    
    function ContentEditable($sce) {
        // Usage:
        //     <ANY contenteditable></ANY>
        // Creates:
        // 
        var directive = {
            restrict: "A", // only activate on element attribute
            require: "?ngModel", // get a hold of NgModelController
            link: link
        };
        return directive;

        function link(scope, element, attrs, ngModel) {
            if (!ngModel) return; // do nothing if no ng-model

            // Specify how UI should be updated
            ngModel.$render = function() {
                element.html($sce.getTrustedHtml(ngModel.$viewValue || ""));
            };

            // Listen for change events to enable binding
            element.on("blur keyup change", function() {
                scope.$evalAsync(read);
            });
            ngModel.$render(); // initialize

            // Write data to the model
            function read() {

                var html = element.html();
                // When we clear the content editable the browser leaves a <br> behind
                // If strip-br attribute is provided then we strip this out
                if (attrs.stripBr && html === "<br>") {
                    html = "";
                }
                ngModel.$setViewValue(html);
            }
        }
    }

})();