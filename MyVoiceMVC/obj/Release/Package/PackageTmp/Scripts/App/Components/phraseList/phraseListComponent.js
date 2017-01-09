(function () {
    "use strict";

    PhraseListController.$inject = ['$uibModal', '$filter', 'SoundService'];
    function PhraseListController($uibModal, $filter, ss) {
        var $ctrl = this;

        //properties
        $ctrl.newTag = "";
        $ctrl.checkedTags = [];
        $ctrl.sortProperty = {
            type: 'text'
        };

        //events
        $ctrl.$onInit = function () {
            //console.log($ctrl);
            $ctrl.phrases = $filter('selectedCategory')($ctrl.allPhrases, $ctrl.categoryFilter);
            angular.forEach($ctrl.allTags, function (tag) {
                $ctrl.checkedTags.push({
                    Checked: doesHaveTag(tag),
                    Tag: tag
                })
            });
        }

        $ctrl.$onChanges = function (changesObj) {
            //$ctrl.editMode = changesObj.editMode.currentValue;
            //$ctrl.phrases = $filter('selectedCategory')($ctrl.allPhrases, $ctrl.categoryFilter);
            //var desiredFilter = $filter('selectedCategory');
        }
                
        $ctrl.btnPlay = function (phrase, $event) {

            //fix any phrases that are in a broken state. (interrupted before finished loading)
            angular.forEach($ctrl.phrases, function (aPhrase, key) {
                aPhrase.loading = false;
                aPhrase.playProgress = null;
            });

            if (!$ctrl.editMode && phrase != null) {
                ss.playSound(phrase, true, $event);
            } else {
                var modalInstance = $uibModal.open({
                    animation: true,
                    component: 'editPhraseComponent',
                    size: 'md',
                    backdrop: 'static',
                    keyboard: false,
                    resolve: {
                        phrase: function () {
                            return phrase;
                        }
                    }
                });
            }
        }
    }

    angular.module("Phrases").component('phraseList', {
        templateUrl: '/Scripts/App/Components/PhraseList/phraseListComponent.html',
        require: '^phrases',
        bindings: {
            categoryFilter: '<',
            editMode: '<',
            allPhrases: '<'
        },
        controller: PhraseListController
    });
})();