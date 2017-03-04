(function () {
    "use strict";

    angular
        .module("ISD.Controllers")
        .controller("ISD.Controllers.ImageLibrary", ImageLibrary);

    ImageLibrary.$inject = ["$location", "$log", "ISD.Services.TagService"];

    function ImageLibrary($location, $log, tagService) {
        /* jshint validthis:true */
        var vm = this;
        
        vm.getTags = _getTags;
        vm.addTag = _addTag;
        vm.deleteTag = _deleteTag;
        vm.draggedTag = null;

        _activate();

        function _activate()
        {
        }


        function _getTags(){
            return tagService.GetTags();
        }

        function _addTag(name){
            return tagService.CreateTag(name);
        }

        function _deleteTag(id) {
            return tagService.DeleteTag(id);
        }
    }
})();
