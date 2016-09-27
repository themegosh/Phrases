﻿(function () {
    "use strict";

    PhrasesController.$inject = ['$scope', 'SoundService', 'ApiService', 'PhrasesService', '$uibModal'];
    function PhrasesController($scope, ss, api, ps, $uibModal) {
        var $ctrl = this;

        //properties
        $ctrl.editMode = false;
        $ctrl.categoryFilter = {
            name: "All"
        };
        
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
                if (category.name === 'All')
                    $ctrl.categoryFilter = {
                        name: "All"
                    };
                else
                    angular.copy(category, $ctrl.categoryFilter);
            } else {
                if (category.name === 'All')
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
                $ctrl.categoryFilter.name = "All";
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