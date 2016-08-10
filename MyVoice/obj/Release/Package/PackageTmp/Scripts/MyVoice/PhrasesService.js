(function () {
    "use strict";
    angular.module("MyVoice").factory("PhrasesService", [ function () {

        var service = {};

        service.phrases = [];
        service.phraseIndex = 1;
        service.tags = [];

        service.assignAllPhrases = function (inPhrases) {
            service.phrases.length = 0;//clear the old ones
            service.phraseIndex = 1;
            angular.forEach(inPhrases, function (phrase) {
                addSoundManagerProperties(phrase);
                service.phrases.push(phrase);
                service.phraseIndex++;
            });
        }

        service.add = function (phrase) {
            addSoundManagerProperties(phrase);
            service.phrases.push(phrase);
            service.phraseIndex++;
            refreshTags();
        }
        
        service.delete = function (phrase) {
            angular.forEach(service.phrases, function (aPhrase, key) {
                if (aPhrase.Text === phrase.Text) {
                    service.phrases.splice(key, 1); //remove this one
                }
            });
            refreshTags();
        }

        service.update = function (newPhrase, oldPhrase) {

        }
        
        //private functions
        function refreshTags() {
            angular.forEach(service.phrases, function (aPhrase, key) {
                angular.forEach(aPhrase.Tags, function (tag) {
                    if ($.inArray(tag, service.tags) == -1) {
                        service.tags.push(tag);
                    }
                });
            });
        }

        function addSoundManagerProperties(phrase) {
            //required attributes for angular sound-manager
            phrase.id = service.phraseIndex;
            phrase.title = phrase.Text;
            phrase.artist = "john doe";
            phrase.url = "/tts/" + md5(phrase.Text) + ".ogg";
        }

        return service;

    }]);
})();