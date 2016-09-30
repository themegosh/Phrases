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
                        active: false,
                        justChanged: false,
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

                if (!$ctrl.phrase.customAudio) { //fix out of date stuff
                    $ctrl.phrase.customAudio = {
                        active: false,
                        justChanged: false,
                        uploadedName: ""
                    }
                }
            }
            //other initializations
            angular.copy(ps.categories, $ctrl.checkedCategories);
            angular.forEach($ctrl.checkedCategories, function (category) {
                category.checked = $ctrl.hasCategory(category);
            });
            $ctrl.customAudioStatus = $ctrl.phrase.customAudio.active ? "Using custom audio: " + $ctrl.phrase.customAudio.uploadedName : "Using text-to-speech";
            //angular.copy(ps.categories, $ctrl.categories);

            console.log($ctrl);
        }
        
        $ctrl.btnUploadClick = function () {
            $('.audio-hidden-input')[0].click();
        }

        $ctrl.btnQuickPhrase = function () {
            if ($ctrl.isClean) {
                if ($ctrl.phrase.tempGuid)
                    ss.playSound($ctrl.phrase.tempGuid);
                else
                    ss.playSound($ctrl.phrase.guid);
            } else {
                $ctrl.isClean = true;
                api.quickPhrase($ctrl.phrase, true);
            }
        }

        $ctrl.save = function () {
            if ($ctrl.phrase.customAudio.active === true && $ctrl.phrase.customAudio.uploadedName === "") {
                $confirm({ text: 'You have checked "Use custom audio" but not uploaded a file to use... Tap "Cancel" and upload a custom audio file, or "Ok" to save using text-to-speech.' })
                    .then(function () {
                        //update the categories to align with the checked ones
                        delete $ctrl.phrase.tempGuid; //remove unneeded data
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
                    });
            } else {
                //update the categories to align with the checked ones
                delete $ctrl.phrase.tempGuid; //remove unneeded data
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
            if (!$ctrl.phrase.customAudio.active) { //disabling custom audio
                $confirm({ text: 'Deactivating this will delete the current audio file and use text-to-speech again.' })
                    .then(function () {
                        $ctrl.phrase.customAudio.justChanged = true;
                        $ctrl.phrase.customAudio.uploadedName = "";
                    });
            } else { //enabling it
                $ctrl.phrase.customAudio.justChanged = true;
            }
        }

        $ctrl.customFileChanged = function () {
            $ctrl.customAudioStatus = "Uploading...";

            upload({
                url: '/api/tts/CustomAudioUpload',
                method: 'POST',
                data: {
                    phrase: angular.toJson($ctrl.phrase, false),
                    aFile: $("#customAudioFile"), // a jqLite type="file" element, upload() will extract all the files from the input and put them into the FormData object before sending.
                }
            }).then(
                function (response) {
                    showNotification("Success", "Audio uploaded!", "success");
                    console.log(response.data); // will output whatever you choose to return from the server on a successful upload
                    angular.copy(angular.fromJson(response.data), $ctrl.phrase);//update the current model
                    $ctrl.phrase.customAudio.active = true;
                    $ctrl.customAudioStatus = "Using custom audio: " + $ctrl.phrase.customAudio.uploadedName;
                },
                function (response) {
                    showNotification("Error", "Audio could not be uploaded...", "danger");
                    $ctrl.customAudioStatus = "Custom audio upload failed...";
                    console.error(response); //  Will return if status code is above 200 and lower than 300, same as $http
                }
            );
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