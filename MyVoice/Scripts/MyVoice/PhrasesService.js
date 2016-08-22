(function () {
    "use strict";
    angular.module("MyVoice").factory("PhrasesService", [ function () {

        var service = {};

        service.phrases = [];
        service.phraseIndex = 1;
        service.tags = [];
        service.tagFilter = "All"

        service.assignAllPhrases = function (inPhrases) {
            service.phrases.length = 0;//clear the old ones
            service.phraseIndex = 1;
            angular.forEach(inPhrases, function (phrase) {
                service.addSoundManagerProperties(phrase);
                service.phrases.push(phrase);
                service.phraseIndex++;
            });
            service.refreshTags();
        }

        service.add = function (phrase) {
            service.addSoundManagerProperties(phrase);
            service.phrases.push(phrase);
            service.phraseIndex++;
            service.refreshTags();
        }
        
        service.delete = function (phrase) {
            angular.forEach(service.phrases, function (aPhrase, key) {
                if (aPhrase.text === phrase.text) {
                    service.phrases.splice(key, 1); //remove this one
                }
            });
            service.refreshTags();
        }

        service.replace = function (newPhrase, oldPhrase) {
            angular.forEach(service.phrases, function (aPhrase, key, phrases) {
                if (aPhrase.text === oldPhrase.text) { //found it
                    //replace it
                    newPhrase.id = service.phraseIndex;
                    service.phraseIndex++;
                    service.phrases[key] = newPhrase;
                }
            });
            service.refreshTags();
        }

        service.addSoundManagerProperties = function (phrase) {
            //required attributes for angular sound-manager
            phrase.id = service.phraseIndex;
            service.phraseIndex++;
            phrase.title = phrase.text;
            phrase.artist = "";
        }
                
        //private functions
        service.refreshTags = function () {
            service.tags.length = 0;
            angular.forEach(service.phrases, function (aPhrase, key) {
                angular.forEach(aPhrase.tags, function (tag) {
                    if ($.inArray(tag, service.tags) == -1) {
                        service.tags.push(tag);
                    }
                });
            });
        }

        return service;

    }]);
})();