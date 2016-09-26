(function () {
    "use strict";

    PhrasesController.$inject = ['$scope', '$element', '$attrs', '$timeout', 'SoundService', 'ApiService', 'PhrasesService', '$rootScope', '$uibModal'];
    function PhrasesController($scope, $element, $attrs, $timeout, ss, api, ps, $rootScope, $uibModal) {
        var $ctrl = this;

        //properties
        $ctrl.editMode = false;
        $ctrl.categoryFilter = "All";
        
        //events
        $ctrl.$onInit = function () {
            api.getUserData();
            $ctrl.categories = ps.categories;
            $ctrl.phrases = ps.phrases;
            ps.resizeScrollbar();
            $(window).resize(ps.resizeScrollbar);
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

        

        $ctrl.btnPlayPause = function () {
            ss.playPause();
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