(function () {
    "use strict";

    EditPhrasesController.$inject = ['ApiService', 'PhrasesService', 'SoundService'];
    function EditPhrasesController(api, ps, ss) {
        var $ctrl = this;

        //properties
        $ctrl.checkedCategories = [];
        $ctrl.phrase = {};

        //events
        $ctrl.$onInit = function () {
            if ($ctrl.resolve.phrase == null) {
                $ctrl.modalTitle = "New Phrase";
                $ctrl.phrase = {
                    text: "",
                    categories: []
                };
                $('#txtPhrase').ready(function () {
                    $('#txtPhrase').focus();
                    $('#txtPhrase').click();
                });
            } else {
                $ctrl.modalTitle = "Edit Phrase";
                angular.copy($ctrl.resolve.phrase, $ctrl.phrase);
                
                //$ctrl.phrase = $ctrl.resolve.phrase;
            }
            angular.copy(ps.categories, $ctrl.checkedCategories);
            //angular.copy(ps.categories, $ctrl.categories);

            angular.forEach($ctrl.checkedCategories, function (category) {
                category.checked = $ctrl.hasCategory(category);
            });
            console.log($ctrl);
        }
        
        $ctrl.btnUploadClick = function () {
            $('.audio-hidden-input')[0].click();
        }

        $ctrl.btnQuickPhrase = function () {
            if ($ctrl.phrase.isClean) {
                if ($ctrl.phrase.tempGuid)
                    ss.playSound($ctrl.phrase.tempGuid);
                else
                    ss.playSound($ctrl.phrase.guid);
            } else {
                $ctrl.phrase.isClean = true;
                api.quickPhrase($ctrl.phrase, true);
            }
        }

        $ctrl.ok = function () {
            //update the categories to align with the checked ones
            delete $ctrl.phrase.tempGuid; //remove unneeded data
            delete $ctrl.phrase.isClean;
            $ctrl.phrase.categories.length = 0;
            angular.forEach($ctrl.checkedCategories, function (category) {
                if (category.checked) {
                    $ctrl.phrase.categories.push(category.guid);
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
                if (aCategory.guid == category)
                    hasCategory = true;
            });
            return hasCategory;
        }

        //$ctrl.$on("fileSelected", function (event, args) {
        //    $scope.$apply(function () {            
        //        //add the file object to the scope's files collection
        //        $scope.files.push(args.file);
        //    });
        //})

        //$ctrl.uploadFile = function () {
        //    var fd = new FormData();
        //    for (var i in scope.files) {
        //        fd.append("uploadedFile", scope.files[i]);
        //    }
        //    var xhr = new XMLHttpRequest();
        //    xhr.upload.addEventListener("progress", uploadProgress, false);
        //    xhr.addEventListener("load", uploadComplete, false);
        //    xhr.addEventListener("error", uploadFailed, false);
        //    xhr.addEventListener("abort", uploadCanceled, false);
        //    xhr.open("POST", "/fileupload");
        //    xhr.send(fd);
        //}

        //function uploadProgress(evt) {
        //    scope.$apply(function () {
        //        if (evt.lengthComputable) {
        //            scope.progress = Math.round(evt.loaded * 100 / evt.total)
        //        } else {
        //            scope.progress = 'unable to compute'
        //        }
        //    })
        //}

        //function uploadComplete(evt) {
        //    /* This event is raised when the server send back a response */
        //    alert(evt.target.responseText)
        //}

        //function uploadFailed(evt) {
        //    alert("There was an error attempting to upload the file.")
        //}

        //function uploadCanceled(evt) {
        //    scope.$apply(function () {
        //        scope.progressVisible = false
        //    })
        //    alert("The upload has been canceled by the user or the browser dropped the connection.")
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