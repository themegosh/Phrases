(function () {
    "use strict";

    PhraseListController.$inject = ['ApiService', 'PhrasesService', '$uibModal'];
    function PhraseListController(api, ps, $uibModal) {
        var $ctrl = this;

        //properties
        $ctrl.sortableOptions = {
            disabled: true,
            axis: 'y',
            update: function (e, ui) { console.log("dragged"); },
        };
        $ctrl.newTag = "";
        $ctrl.checkedTags = [];

        //events
        $ctrl.$onInit = function () {
            console.log("$onInit PhraseListController");
            console.log($ctrl);

            $ctrl.sortableOptions.disabled = $ctrl.editMode;

            angular.forEach($ctrl.allTags, function (tag) {
                $ctrl.checkedTags.push({
                    Checked: doesHaveTag(tag),
                    Tag: tag
                })
            });
        }

        $ctrl.$onChanges = function (changesObj) {
            $ctrl.editMode = changesObj.editMode.currentValue;
            //console.log(changesObj);
            //if (changesObj.editMode) {
            //    var prefix;
            //    (changesObj.editMode.currentValue === 'Pascal') ?
            //      prefix = 'Howdy ' : prefix = 'Hello ';
            //    this.name = prefix + this.name;
            //}
        }

        $ctrl.btnCreateNewPhrase = function () {
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

            console.log("btnClicked!");
            $ctrl.newPhrase.text = $ctrl.newPhrase.text.trim();
            if ($ctrl.newPhrase.text.length) {
                api.savePhrase($ctrl.newPhrase);
                console.log($ctrl.newPhrase);
                $ctrl.newPhrase.text = "";
            }
        }

        $ctrl.btnPlay = function (phrase) {
            if (!$ctrl.editMode) {
                try {
                    angularPlayer.addTrack(phrase);
                    angularPlayer.playTrack(phrase.id);
                    //angularPlayer.clearPlaylist(function () {
                    //    angularPlayer.addTrack(phrase);
                    //    angularPlayer.playTrack(phrase.id);
                    //});
                }
                catch (ex) {
                    console.log(ex);
                    showNotification("Error", ex, "error");
                }
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
        bindings: {
            categoryFilter: '<',
            editMode: '<',
            phrases: '<'
        },
        controller: PhraseListController
    });
})();