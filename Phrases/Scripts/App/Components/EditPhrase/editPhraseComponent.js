(function () {
    "use strict";

    EditPhrasesController.$inject = ['ApiService', 'PhrasesService', 'angularPlayer'];
    function EditPhrasesController(api, ps, angularPlayer) {
        var $ctrl = this;

        //properties
        $ctrl.checkedCategories = [];
        $ctrl.categories = [];

        //events
        $ctrl.$onInit = function () {
            console.log("$onInit EditPhrasesController");
            if ($ctrl.resolve.phrase == null) {
                $ctrl.modalTitle = "New Phrase";
                $ctrl.phrase = {
                    text: "",
                    categories: [],
                    hash: ""
                };
                $('#txtPhrase').ready(function () {
                    $('#txtPhrase').focus();
                    $('#txtPhrase').click();
                });
            } else {
                $ctrl.modalTitle = "Edit Phrase";
                $ctrl.phrase = $ctrl.resolve.phrase;
            }
            angular.copy(ps.categories, $ctrl.categories);
            //$ctrl.categories = angular.copy(ps.categories);

            angular.forEach($ctrl.categories, function (category) {
                $ctrl.checkedCategories.push({
                    checked: $ctrl.hasCategory(category),
                    name: category.name
                })
            });

            console.log(ps.categories);
            console.log($ctrl);
        }
        
        $ctrl.btnQuickPhrase = function () {
            if ($ctrl.phrase.isClean) {
                angularPlayer.addTrack($ctrl.phrase);
                angularPlayer.playTrack($ctrl.phrase.id);
            } else {
                $ctrl.phrase.isClean = true;
                api.quickPhrase($ctrl.phrase);
            }
        }

        $ctrl.ok = function () {
            //update the categories to align with the checked ones
            $ctrl.phrase.categories.length = 0;
            angular.forEach($ctrl.checkedCategories, function (category) {
                if (category.checked) {
                    $ctrl.phrase.categories.push(category.name);
                }
            });
            if ($ctrl.phrase.text !== "") {
                api.savePhrase($ctrl.phrase);
            }
            $ctrl.dismiss({ $value: 'cancel' });
        }

        $ctrl.cancel = function () {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        $ctrl.delete = function () {
            api.deletePhrase($ctrl.phrase);
            $ctrl.dismiss({ $value: 'cancel' });
        }

        $ctrl.hasCategory = function (aCategory) {
            var hasCategory = false;
            angular.forEach($ctrl.phrase.categories, function (category) {
                if (aCategory.name == category)
                    hasCategory = true;
            });
            return hasCategory;
        }

        //$ctrl.addTag = function () {
        //    if ($.inArray($ctrl.newTag, $ctrl.phrase.tags) == -1) {
        //        $ctrl.phrase.tags.push($ctrl.newTag);
        //        $ctrl.checkedTags.push({
        //            Checked: true,
        //            Tag: $ctrl.newTag
        //        })
        //    }
        //    else {
        //        showNotification("Error", "Category already exists!", "error");
        //    }
        //    $ctrl.newTag = "";
        //}

        //$ctrl.checkChanged = function (tag) {
        //    if (tag.Checked === false) {
        //        removeA($ctrl.phrase.tags, tag.Tag);
        //    } else {
        //        $ctrl.phrase.tags.push(tag.Tag);
        //    }
        //}

        

        //function removeA(arr) {
        //    var what, a = arguments, L = a.length, ax;
        //    while (L > 1 && arr.length) {
        //        what = a[--L];
        //        while ((ax = arr.indexOf(what)) !== -1) {
        //            arr.splice(ax, 1);
        //        }
        //    }
        //    return arr;
        //}
    }

    angular.module("Phrases").component('editPhraseComponent', {
        templateUrl: '/Scripts/App/Components/EditPhrase/editPhraseModal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: EditPhrasesController
    });
})();