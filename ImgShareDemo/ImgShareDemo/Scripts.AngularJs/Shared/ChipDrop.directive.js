(function() {
    "use strict";

    angular
        .module("ISD.Directives")
        .directive("chipDrop", ChipDrop);

    ChipDrop.$inject = ["$window", "$log", "$parse"];
    
    function ChipDrop($window, $log, $parse) {
        var directive = {
            restrict: "A",
            scope: true,
            link: link
        };
        return directive;

        function link(scope, element, attrs) {
            scope.chipsGetter = $parse(attrs.chips);
            scope.currentlyDraggedChipGetter = $parse(attrs.currentlyDraggedChip);
            scope.onChipAddGetter = $parse(attrs.chipAdded);
            scope.canDrop = false;
            scope.$parent.$watch(attrs.currentlyDraggedChip, function (newVal, oldVal) {
                if (newVal !== oldVal)
                {
                    if(newVal)
                    {
                        var chips = getChips() || [];
                        scope.canDrop = !chips.some(function (chip) { return chip.Id === newVal.Id; });
                        element.addClass("drop-box");
                    }
                    else
                    {
                        scope.canDrop = false;
                        element.removeClass("drop-box");
                    }
                }
            });
            element.on("dragover", function (event) {
                if (scope.canDrop)
                {
                    event.preventDefault();
                    // Set the dropEffect to move
                    event.originalEvent.dataTransfer.dropEffect = "move"
                }
            });

            element.on("drop", function (event) {
                // reset the transparency
                var data = getcurrentlyDraggedChip();
                $log.info(data);
                $log.info("candrop", scope.canDrop);
                if (scope.canDrop && data) {
                    var chips = getChips();
                    if (!_.isArray(chips)) {
                        throw "chips parameter must be an array";
                    }

                    if (data && !chips.some(function (chip) { return chip.Id === data.Id; })) {
                        event.preventDefault();
                        getChipAdded(data);
                        scope.$apply(function () {
                            chips.push(angular.fromJson(data));
                        });
                    }
                }
                event.target.style.opacity = "";
            });
            function getChips() {
                return scope.chipsGetter(scope.$parent);
            }

            function getcurrentlyDraggedChip() {
                return scope.currentlyDraggedChipGetter(scope.$parent);
            };

            function getChipAdded(chip) {
                return scope.onChipAddGetter(scope.$parent, { $chip: chip });
            };
        }
    }
})();