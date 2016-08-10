(function () {
    "use strict";
    angular.module("MyVoice").controller("editPhraseCtrl",
        ["$scope", "$uibModalInstance", 'ApiService', 'PhrasesService', 'items',
        function ($scope, $uibModalInstance, ps, items) {

            $scope.phrase = angular.copy(items);
            $scope.allTags = ps.tags;
            $scope.newTag = "";

            $scope.addTag = function () {
                if ($.inArray($scope.newTag, $scope.phrase.Tags) == -1) {
                    ps.tags.push($scope.newTag);
                    $scope.phrase.Tags.push($scope.newTag);
                }
                else {
                    showNotification("Error", "Category already exists!", "error");
                }
            }

            $scope.ok = function () {
                if (items != $scope.phrase) {
                    ps.add($scope.phrase);
                    ps.delete(items);
                }

                $uibModalInstance.dismiss('cancel');
            }

            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            }
            
        }]);
})();