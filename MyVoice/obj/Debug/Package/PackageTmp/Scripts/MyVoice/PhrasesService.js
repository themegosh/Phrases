(function () {
    "use strict";

    angular.module("MyVoice").factory("PhrasesService", [ function () {

        var service = {};

        service.phrases = [];

        service.add = function (phrase) {
            service.addSoundProperties(phrase);
            service.phrases.push(phrase);
            console.log("added:");
            console.log(phrase);
        }

        //workaround for angular sound-manager
        service.addSoundProperties = function (phrase) {
            phrase.id = service.phrases.length + 1;
            phrase.title = phrase.Text;
            phrase.artist = "john doe";
            phrase.url = "/tts/" + md5(phrase.Text) + ".ogg";
        }


        return service;

    }]);
})();