(function () {
    "use strict";

    EditPhrasesController.$inject = ['ApiService', 'PhrasesService'];
    function EditPhrasesController(api, ps) {
        var $ctrl = this;

        //properties
        $ctrl.newTag = "";
        $ctrl.checkedTags = [];

        //events
        $ctrl.$onInit = function () {
            console.log("$onInit EditPhrasesController");
            $ctrl.phrase = $ctrl.resolve.phrase;//angular.copy(items);
            $ctrl.allTags = angular.copy(ps.tags);

            angular.forEach($ctrl.allTags, function (tag) {
                $ctrl.checkedTags.push({
                    Checked: doesHaveTag(tag),
                    Tag: tag
                })
            });
        }

        $ctrl.addTag = function () {
            if ($.inArray($ctrl.newTag, $ctrl.phrase.tags) == -1) {
                $ctrl.phrase.tags.push($ctrl.newTag);
                $ctrl.checkedTags.push({
                    Checked: true,
                    Tag: $ctrl.newTag
                })
            }
            else {
                showNotification("Error", "Category already exists!", "error");
            }
            $ctrl.newTag = "";
        }

        $ctrl.ok = function () {
            //save a tag that hasn't been "Added" yet
            if ($ctrl.newTag !== "") {
                $ctrl.addTag();
            }
            api.savePhrase($ctrl.phrase);
            $ctrl.dismiss({ $value: 'cancel' });
        }

        $ctrl.cancel = function () {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        $ctrl.delete = function () {
            api.deletePhrase($ctrl.phrase);
            $ctrl.dismiss('cancel');
        }

        $ctrl.checkChanged = function (tag) {
            if (tag.Checked === false) {
                removeA($ctrl.phrase.tags, tag.Tag);
            } else {
                $ctrl.phrase.tags.push(tag.Tag);
            }
        }

        function doesHaveTag(aTag) {
            var hasTag = false;
            angular.forEach($ctrl.phrase.tags, function (tag) {
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