(function () {
    "use strict";
    angular.module("MyVoice").controller("editPhraseCtrl",
        ["$scope", "$uibModalInstance", 'ApiService', 'PhrasesService', 'items',
        function ($scope, $uibModalInstance, api, ps, items) {

            $scope.phrase = angular.copy(items);
            $scope.allTags = angular.copy(ps.tags);
            $scope.checkedTags = []
            $scope.newTag = "";

            $scope.init = function () {
                console.log("init");
                angular.forEach($scope.allTags, function (tag) {
                    $scope.checkedTags.push({
                        Checked: doesHaveTag(tag),
                        Tag: tag
                    })
                });

                
            }

            $scope.addTag = function () {
                if ($.inArray($scope.newTag, $scope.phrase.tags) == -1) {
                    $scope.phrase.tags.push($scope.newTag);
                    $scope.checkedTags.push({
                        Checked: true,
                        Tag: $scope.newTag
                    })
                }
                else {
                    showNotification("Error", "Category already exists!", "error");
                }
                $scope.newTag = "";
            }

            $scope.ok = function () {
                api.savePhrase($scope.phrase);
                $uibModalInstance.dismiss('cancel');
            }

            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            }

            $scope.delete = function () {
                api.deletePhrase(items);
                $uibModalInstance.dismiss('cancel');
            }

            $scope.checkChanged = function (tag) {
                if (tag.Checked === false) {
                    removeA($scope.phrase.tags, tag.Tag);
                } else {
                    $scope.phrase.tags.push(tag.Tag);
                }
            }

            function doesHaveTag(aTag) {
                var hasTag = false;
                angular.forEach($scope.phrase.tags, function (tag) {
                    if (aTag == tag)
                        hasTag = true;
                });
                return hasTag;
            }

            function removeA(arr) {
                var what, a = arguments, L = a.length, ax;
                while (L > 1 && arr.length) {
                    what = a[--L];
                    while ((ax = arr.indexOf(what)) !== -1) {
                        arr.splice(ax, 1);
                    }
                }
                return arr;
            }
            
        }]);
})();