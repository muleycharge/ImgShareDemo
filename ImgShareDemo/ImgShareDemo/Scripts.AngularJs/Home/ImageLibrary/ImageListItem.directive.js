(function() {
	"use strict";

	angular
		.module("ISD.Directives")
		.directive("imageListItem", ImageListItem);

	ImageListItem.$inject = ["$window", "$log", "$uibModal", "$sce"];
	
	function ImageListItem($window, $log, $uibModal, $sce) {
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
		    scope.imageListItemCtrl.imageList = imageList;
		}

		function controller($scope) {
		    var vm = this;
			vm.loading = false;
			vm.loadProgress = 0.0;
			vm.imageFile = null;
			vm.popoverTemplate = "sharePopoverTemplate.html";

			vm.Delete = _delete;
			vm.Share = _share;
			vm.Upload = _upload;
			vm.View = _view;
			vm.ChipAdded = _chipAdded;
			vm.GetShareView = _getShareView;

			$scope.$watch("imageListItemCtrl.Asset.Name", function (newVal, oldVal) {
				if (newVal !== oldVal) {
					vm.imageList.Update(vm.Asset);
				}
			});
			$scope.$watch("imageListItemCtrl.Asset.Description", function (newVal, oldVal) {
				if (newVal !== oldVal) {
					vm.imageList.Update(vm.Asset);
				}
			});

			function _delete(){
				vm.imageList.Delete(vm.Asset);
			}

			function _share(){

			}

			function _update() {
				var promise = vm.imageList.Update(vm.Asset)
					.then(function(response){
						if(_.has(response, "Data.Id"))
						{
							vm.Asset.Id = response.Data.Id;
						}
						else {
							$log.error("Invalid update response is missiong Id.")
							$log.error(response);
							response.reject();
						}
					});
				return promise;
			}

			function _upload($file) {
				if ($file)
				{
					vm.loading = true;
					var promise;
					if (vm.Asset.Id) {
						promise = vm.imageList.Upload(vm.Asset, $file, _uploadStatus);
					}
					else {
						promise = _update().then(function (result) {
							return vm.imageList.Upload(vm.Asset, $file, _uploadStatus);
							
						});
					}
					return promise.then(function (response) {
							if (_.has(response, "data.Data.SourceUrl")) {
								vm.Asset.SourceUrl = response.data.Data.SourceUrl;
							}
							else {
								$log.error("Invalid update response is missiong SourceUrl.")
								$log.error(response);
								response.reject();
							}
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

			function _chipAdded(chip)
			{
			    var promise = vm.imageList.AddTag(vm.Asset.Id, chip);
			    promise.then(
                    function (response) { },
                    function (response) {
                        _.remove( vm.Asset.Tags, { Id: chip.Id });
                    });
			}

			function _view() {
				var modalInstance = $uibModal.open({
					locals: { Asset: vm.Asset },
					templateUrl: "/Scripts.AngularJs/Home/ImageLibrary/ImageListItem.modal.html",
					controller: function ($scope) {
						$scope.Asset = vm.Asset;
						$scope.ok = modalInstance.close
					}
				});

			}

			function _getShareView() {
			    return ("<input class='form-control' value='" + vm.Asset.SourceUrl + "' />");
			}

			function _waitUpdate(Asset){
				

			}
		}
	}

})();