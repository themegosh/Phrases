(function () {
    "use strict";
    EditPhrasesController.$inject = ['ApiService', 'PhrasesService', 'SoundService', 'upload', '$confirm'];
    function EditPhrasesController(api, ps, ss, upload, $confirm) {
        var $ctrl = this;

        //properties
        $ctrl.checkedCategories = [];
        $ctrl.phrase = {};

        //events
        $ctrl.$onInit = function () {
            if ($ctrl.resolve.phrase == null) { //new phrase
                $ctrl.modalTitle = "New Phrase";
                $ctrl.phrase = {
                    text: "",
                    categories: [],
                    customAudio: {
                        hasCustomAudio: false,
                        uploadedName: ""
                    }
                };
                $('#txtPhrase').ready(function () {
                    $('#txtPhrase').focus();
                    $('#txtPhrase').click();
                });
            } else { //editing a phrase
                $ctrl.modalTitle = "Edit Phrase";
                angular.copy($ctrl.resolve.phrase, $ctrl.phrase);

                //do we need to clear it?
                //$ctrl.phrase.customAudio = {
                //    hasCustomAudio: false,
                //    uploadedName: ""
                //}

            }
            //other initializations
            angular.copy(ps.categories, $ctrl.checkedCategories);
            angular.forEach($ctrl.checkedCategories, function (category) {
                category.checked = $ctrl.hasCategory(category);
            });
            $ctrl.customAudioStatus = $ctrl.phrase.customAudio.hasCustomAudio ? "Using custom audio: " + $ctrl.phrase.customAudio.name : "Using text-to-speech";
            //angular.copy(ps.categories, $ctrl.categories);

            console.log($ctrl);
        }
        
        $ctrl.btnUploadClick = function () {
            $('.audio-hidden-input')[0].click();
        }

        $ctrl.btnQuickPhrase = function () {
            if ($ctrl.isClean) {
                ss.playPause();
                //ss.playSound($ctrl.phrase);
            } else {
                $ctrl.isClean = true;
                api.quickPhrase($ctrl.phrase, true);
            }
        }

        $ctrl.save = function () {
            if ($ctrl.phrase.customAudio.hasCustomAudio === true && !$ctrl.phrase.customAudio.name) {
                $confirm({ text: 'You have checked "Use custom audio" but have not uploaded a file to use... Tap "Cancel" and upload a custom audio file, or "Ok" to save using text-to-speech.' })
                    .then(function () {
                        $ctrl.phrase.customAudio.hasCustomAudio = false;
                        $ctrl.saveAndClose();
                    });
            } else {
                $ctrl.saveAndClose();
            }
        }

        $ctrl.saveAndClose = function () {
            delete $ctrl.phrase.tempGuid; //remove unneeded data
            //update the categories to align with the checked ones
            $ctrl.phrase.categories.length = 0;
            angular.forEach($ctrl.checkedCategories, function (category) {
                if (category.checked) {
                    $ctrl.phrase.categories.push(category.guid);
                }
            });
            if ($ctrl.phrase.text !== "") {
                api.savePhrase($ctrl.phrase);
            }
            ss.forceReload = true;
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

        $ctrl.usesCustomAudioToggle = function () {
            if (!$ctrl.phrase.customAudio.hasCustomAudio) { //disabling custom audio
                $ctrl.phrase.customAudio.hasCustomAudio = true;
                $confirm({ text: 'Deactivating this will delete the current audio file and use text-to-speech again.' })
                    .then(function () {
                        $ctrl.customAudioStatus = "Using text-to-speech";
                        $ctrl.phrase.customAudio.name = "";
                        $ctrl.phrase.customAudio.hasCustomAudio = false;
                        $ctrl.phrase.forceRefresh = true;
                    });
            } else { //enabling it
                //$ctrl.phrase.customAudio.justChanged = true;
            }
        }

        $ctrl.customFileChanged = function () {
            $ctrl.customAudioStatus = "Uploading...";

            upload({
                url: '/api/audio/CustomAudioUpload',
                method: 'POST',
                data: {
                    userGuid: ps.user.userGuid,
                    phrase: angular.toJson($ctrl.phrase, false),
                    aFile: $("#customAudioFile"), // a jqLite type="file" element, upload() will extract all the files from the input and put them into the FormData object before sending.
                }
            }).then(
                function (response) {
                    showNotification("Success", "Audio uploaded!", "success");
                    console.log(response.data); // will output whatever you choose to return from the server on a successful upload
                    angular.copy(angular.fromJson(response.data), $ctrl.phrase);//update the current model
                    $ctrl.customAudioStatus = "Using custom audio: " + $ctrl.phrase.customAudio.name;
                },
                function (response) {
                    showNotification("Error", "Audio could not be uploaded...", "danger");
                    $ctrl.customAudioStatus = "Custom audio upload failed...";
                    console.error(response); //  Will return if status code is above 200 and lower than 300, same as $http
                }
            );
            ss.forceReload = true;
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