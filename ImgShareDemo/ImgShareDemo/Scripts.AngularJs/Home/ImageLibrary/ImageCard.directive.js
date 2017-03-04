(function() {
    'use strict';

    angular
        .module('ISD.Directives')
        .directive('imageCard', ImageCard);

    ImageCard.$inject = ['$window'];
    
    function ImageCard ($window) {
        // Usage:
        //     <ImageCard></ImageCard>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'EA'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();