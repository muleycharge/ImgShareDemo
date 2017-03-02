(function() {
	"use strict";

	angular
		.module("ISD.Directives")
		.directive("imageListItem", ImageListItem);

	ImageListItem.$inject = ["$window"];
	
	function ImageListItem ($window) {
		// Usage:
		//     <image-list-item asset="{asset}"></image-list-item>
		// Creates:
		// 
		var directive = {
			templateUrl: "/Scripts.AngularJs/Home/ImageLibrary/ImageListItem.directive.html",
			link: link,
			controller: controller,
			bindToController: true,
			controllerAs: "imageListItemCtrl",
			// Require parent scope
			require: "^imageList",
			restrict: "A",
			replace: true,
			scope: {
				Asset: "=asset"
			}
		};
		return directive;

		function link(scope, element, attrs, imageList) {
			// Assign parent scope to this scope.
			scope.imageList = imageList;
		}

		function controller($scope) {
			var vm = this;
			vm.loading = false;
			vm.loadProgress = 0.0;
			vm.imageFile = null;

			vm.Delete = _delete;
			vm.Share = _share;
			vm.Upload = _upload;

			$scope.$watch("imageListItemCtrl.Asset.Name", function (newVal, oldVal) {
				if (newVal !== oldVal) {
					$scope.imageList.Update(vm.Asset);
				}
			});
			$scope.$watch("imageListItemCtrl.Asset.Description", function (newVal, oldVal) {
				if (newVal !== oldVal) {
					$scope.imageList.Update(vm.Asset);
				}
			});

			function _delete(){
				$scope.imageList.Delete(vm.Asset.Id);
			}

			function _share(){

			}

			function _upload($file) {
			    if ($file)
			    {
				    vm.loading = true;
				    var promise = $scope.imageList.Upload(vm.Asset, $file, _uploadStatus)
					    .then(function (response) {
						    vm.Asset.Id = response.data.Data.Id;
						    vm.Asset.SourceUrl = response.data.Data.SourceUrl;
					    })
					    .finally(function () {
					        vm.loading = false;
					        vm.imageFile = null;
					        vm.loadProgress = 0.0;
					    });
			    }
			}

			function _uploadStatus(evt){
				vm.loadProgress = evt.loaded / evt.total;
				console.log(evt.loaded);
			}

			function _waitUpdate(Asset){
				

			}
		}
	}

})();