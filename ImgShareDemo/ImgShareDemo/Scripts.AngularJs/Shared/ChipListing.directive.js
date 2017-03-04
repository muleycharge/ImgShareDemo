(function() {
    "use strict";

    angular
        .module("ISD.Directives")
        .directive("chipListing", ChipListing)
        .directive("chipListingChip", ChipListingChip);

    ChipListing.$inject = ["$window", "$log"];
    ChipListingChip.$inject = ["$window", "$log"];

    function ChipListingChip($window, $log) {
        var directive = {
            template: "<div draggable='true' id='chip-{{chipCtrl.chip.Id}}' class='tag' data-drag='true' ><span>{{chipCtrl.chip.Name}}</span></div>",
            restrict: "E",
            link: link,
            controller: controller,
            controllerAs: "chipCtrl",
            require: "^^chipListing",
            bindToController: true,
            replace: true,
            scope: {
                chip: "=",
                dragContextId: "@"
            }
        };
        return directive;

        function controller() {
            var vm = this;

        }

        function link(scope, element, attrs, chipListCtrl) {


            element.on("dragstart", function (event) {
                var dto = { dragContext: scope.chipCtrl.dragContextId, data: scope.chipCtrl.chip };
                event.target.style.opacity = .5;
                scope.$apply(function () {
                    chipListCtrl.currentlyDraggingChip = scope.chipCtrl.chip;
                });
            });

            element.on("dragend", function (event) {
                // reset the transparency
                scope.$apply(function () {
                    chipListCtrl.currentlyDraggingChip = null;
                });
                event.target.style.opacity = "";
            });

            element.on("drop", function (event) {
                // reset the transparency
                $log.info("drop chipListingChip " + scope.chipCtrl.chip.Id);
            });
        }
    }
    
    function ChipListing($window, $log) {
        // Usage:
        //     <chip-listing></chip-listing>
        // Creates:
        // 
        var directive = {
            templateUrl: "/Scripts.AngularJs/Shared/ChipListing.directive.html",
            restrict: "EA",
            link: link,
            controller: controller,
            controllerAs: "chipListCtrl",
            bindToController: true,
            replace: true,
            scope: {
                _getChips: "&getChips",
                _addChip: "&addChip",
                _deleteChip: "&deleteChip",
                dragContextId: "@",
                currentlyDraggingChip: "="
            }
        };
        return directive;

        function controller($scope)
        {
            var vm = this;

            vm.chips = [];
            vm.loading = false;

            vm.getChips = getChips;
            vm.addChip = addChip;
            vm.deleteChip = deleteChip;
            vm.getButtonState = getButtonState;
            vm.chipFilter = chipFilter;
            vm.loading = false;
            vm.currentlyDraggingChip = null;
            vm.isAddDisabled = isAddDisabled;
            vm.initialize = initialize; // Called in link.

            function initialize()
            {
                getChips();
            }

            function getChips() {
                vm.loading = true;
                var promise = vm._getChips()();
                promise.then(
                    function (result) {
                        if (_.has(result, "Data.Items")) {
                            vm.chips = result.Data.Items;
                        }
                        return result;
                    });
                promise.finally(function () {
                    vm.loading = false;
                });
            }

            function addChip(name) {
                vm.loading = true;
                var promise = vm._addChip()(name);
                promise.then(function (result) {
                    vm.search = "";
                    if(result.Data)
                    {
                        vm.chips.push(result.Data);
                    }
                });

                promise.finally(function () {
                    vm.loading = false;
                });
            }

            function deleteChip(id) {
                vm.loading = true;
                var promise = vm._deleteChip()(id);
                promise.then(function (result) {

                    var i = _.findIndex(vm.chips, function (chip) {
                            return chip.Id == id;
                        });
                    if(i > -1)
                    {
                        vm.chips.splice(i, 1);
                    }
                });

                promise.finally(function () {
                    vm.loading = false;
                });
            }

            function getButtonState() {
                if(vm.loading)
                {
                    return "loading";
                }
                else if(vm.currentlyDraggingChip != null)
                {
                    return "dragging";
                }
                else if(vm.isAddDisabled())
                {
                    return "add-disabled";
                }
                else
                {
                    return "add-enabled";
                }
            };

            function isAddDisabled()
            {
                return !vm.search || ($scope.filteredChips || []).some(function (chip) {
                    return (chip.Name || "").toLowerCase() === (vm.search || "").toLowerCase();
                });
            }

            function chipFilter(chip) {
                return _.includes((chip.Name || "").toLowerCase(), (vm.search || "").toLowerCase());
            }
        }


        function link(scope, element, attrs) {
            scope.chipListCtrl.placeholder = attrs.placeholder;

            var addRemoveDrop = element.find("#addRemoveChip");
            addRemoveDrop.on("dragover", function (event) {
                if (scope.chipListCtrl.currentlyDraggingChip) {
                    event.preventDefault();
                    // Set the dropEffect to move
                    event.originalEvent.dataTransfer.dropEffect = "move"
                }
            });

            addRemoveDrop.on("drop", function (event) {
                if (scope.chipListCtrl.currentlyDraggingChip) {
                    var chipId = scope.chipListCtrl.currentlyDraggingChip.Id;
                    scope.chipListCtrl.deleteChip(chipId);
                }
                // reset the transparency
                event.target.style.opacity = "";
            });
            scope.chipListCtrl.initialize();
        }
    }

})();