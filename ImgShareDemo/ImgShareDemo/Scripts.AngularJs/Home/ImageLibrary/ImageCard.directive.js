(function() {
    'use strict';

    angular
        .module('app')
        .directive('ImageCard', ImageCard);

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