(function () {
    "use strict";

    PhraseListController.$inject = ['ApiService', 'PhrasesService', '$uibModal', '$filter', 'SoundService'];
    function PhraseListController(api, ps, $uibModal, $filter, ss) {
        var $ctrl = this;

        //properties
        $ctrl.newTag = "";
        $ctrl.checkedTags = [];
        $ctrl.sortProperty = {
            type: 'text'
        };

        //events
        $ctrl.$onInit = function () {
            console.log($ctrl);
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

        //todo: https://github.com/a5hik/ng-sortable
        //$ctrl.dragControlListeners = {
        //    //accept: function (sourceItemHandleScope, destSortableScope) {return true},//override to determine drag is allowed or not. default is true.
        //    itemMoved: function (event) {},//Do what you want
        //    orderChanged: function(event) {},//Do what you want
        //    //containment: '#phraseContainer',//optional param.
        //    //clone: true, //optional param for clone feature.
        //    //allowDuplicates: false //optional param allows duplicates to be dropped.
        //};

        //    $scope.dragControlListeners1 = {
        //        //containment: '#board',//optional param.
        //        //allowDuplicates: true //optional param allows duplicates to be dropped.
        //};

        
        $ctrl.btnPlay = function (phrase) {
            if (!$ctrl.editMode && phrase != null) {
                ss.playSound(phrase);
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