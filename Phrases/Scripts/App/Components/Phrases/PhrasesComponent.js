(function () {
    "use strict";

    PhrasesController.$inject = ['$scope', '$element', '$attrs', '$timeout', 'angularPlayer', 'ApiService', 'PhrasesService', '$rootScope', '$uibModal'];
    function PhrasesController($scope, $element, $attrs, $timeout, angularPlayer, api, ps, $rootScope, $uibModal) {
        var $ctrl = this;

        //properties
        $ctrl.editMode = false;
        $ctrl.categoryFilter = "All";

        $ctrl.slider = {
            curVol: 100,
            options: {
                floor: 0,
                ceil: 100,
                translate: function (value) {
                    return value + '%';
                },
                onChange: function () {
                    console.log("changeVol: " + $ctrl.slider.curVol);
                    angularPlayer.changeVolume($ctrl.slider.curVol);
                }
            }
        };

        //events
        $ctrl.$onInit = function () {
            api.getUserData();
            $ctrl.categories = ps.categories;
            $ctrl.phrases = ps.phrases;
            angularPlayer.changeVolume($ctrl.slider.curVol)
        }

        $ctrl.btnSelectFilter = function (category) {
            if (!$ctrl.editMode) {
                if (category === 'All')
                    $ctrl.categoryFilter = "All";
                else
                    $ctrl.categoryFilter = category.name;
            } else {
                if (category === 'All')
                    return;

                //open category editor
                var modalInstance = $uibModal.open({
                    animation: true,
                    component: 'editCategoryComponent',
                    size: 'md',
                    resolve: {
                        category: function () {
                            return category;
                        }
                    }
                });
            }
        }

        $ctrl.btnEditMode = function () {
            if ($ctrl.editMode === true) {
                $ctrl.editMode = false;
            } else {
                $ctrl.categoryFilter = "All";
                $ctrl.editMode = true;
            }
        }

        $ctrl.btnUser = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'userComponent',
                size: 'sm'
            });
        }

        $ctrl.btnAddPhrase = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'editPhraseComponent',
                size: 'md',
                backdrop: 'static',
                keyboard: false,
                resolve: {
                    phrase: function () {
                        return null;
                    }
                }
            });
        }

        

        $ctrl.btnChangeVolume = function () {
            console.log("btnChangeVolume!");

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: '/Scripts/App/Volume/VolumeModal.html',
                controller: 'volumeCtrl',
                size: 'md'
            });
            modalInstance.rendered.then(function () {
                $rootScope.$broadcast('rzSliderForceRender'); //Force refresh sliders on render. Otherwise bullets are aligned at left side.
            });
        }

    }

    angular.module("Phrases").component('phrases', {
        templateUrl: '/Scripts/App/Components/Phrases/Phrases.html',
        bindings: {
            //stuff: '<'
        },
        controller: PhrasesController
    });
})();