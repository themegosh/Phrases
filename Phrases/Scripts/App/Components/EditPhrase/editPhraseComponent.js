(function () {
    "use strict";

    EditPhrasesController.$inject = ['ApiService', 'PhrasesService'];
    function EditPhrasesController(api, ps) {
        var $ctrl = this;

        //properties
        $ctrl.checkedTags = [];

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
            } else {
                $ctrl.phrase = $ctrl.resolve.phrase;
            }
            $ctrl.categories = angular.copy(ps.categories);

            //angular.forEach($ctrl.categories, function (tag) {
            //    $ctrl.checkedTags.push({
            //        Checked: doesHaveTag(tag),
            //        Tag: tag
            //    })
            //});
        }

        

        $ctrl.ok = function () {
            //save a tag that hasn't been "Added" yet
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

        //function doesHaveTag(aTag) {
        //    var hasTag = false;
        //    angular.forEach($ctrl.phrase.tags, function (tag) {
        //        if (aTag == tag)
        //            hasTag = true;
        //    });
        //    return hasTag;
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