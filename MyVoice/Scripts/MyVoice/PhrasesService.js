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
                if (aPhrase.Text === phrase.Text) {
                    service.phrases.splice(key, 1); //remove this one
                }
            });
            service.refreshTags();
        }

        service.replace = function (newPhrase, oldPhrase) {
            angular.forEach(service.phrases, function (aPhrase, key, phrases) {
                if (aPhrase.Text === oldPhrase.Text) { //found it
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
            phrase.title = phrase.Text;
            phrase.artist = "john doe";
            phrase.url = "/tts/" + md5(phrase.Text) + ".wav";
            phrase.Hash = md5(phrase.Text);
        }
                
        //private functions
        service.refreshTags = function () {
            service.tags.length = 0;
            angular.forEach(service.phrases, function (aPhrase, key) {
                angular.forEach(aPhrase.Tags, function (tag) {
                    if ($.inArray(tag, service.tags) == -1) {
                        service.tags.push(tag);
                    }
                });
            });
        }

        return service;

    }]);
})();