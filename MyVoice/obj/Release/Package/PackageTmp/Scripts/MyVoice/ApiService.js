(function () {
    "use strict";

    angular.module("MyVoice").factory("ApiService", "PhraseService", ['$http', function ($http, ps) {

        var api = {};

        api.addPhrase = function (phrase) {
            console.log(phrase);
            $http({
                method: 'Post',
                url: '/api/tts/SavePhrase',
                data: JSON.stringify(phrase)
            }).then(function successCallback(response) {
                console.log("savePhrase SUCCESS:");
                console.log(response);
                ps.add(angular.fromJson(response.data));
                showNotification("Success", "Phrase saved.", "success");
            }, function errorCallback(response) {
                console.log("savePhrase FAIL:");
                console.log(response);
                showNotification("Error", response.data, "error");
            });
        }

        api.getAllPhrases = function () {
            $http({
                method: 'Get',
                url: '/api/tts/GetAllPhrases'
            }).then(function successCallback(response) {
                console.log("getAllPhrases SUCCESS: ");
                console.log(response);
                ps.assignAllPhrases(angular.fromJson(response.data));
                showNotification("Success", "Phrases retrieved!", "success");
            }, function errorCallback(response) {
                console.log("getAllPhrases FAIL:");
                console.log(response);
                showNotification("Error", response.data, "error");
            });
        }

        api.deletePhrase = function (phrase) {
            console.log(phrase);
            $http({
                method: 'Post',
                url: '/api/tts/DeletePhrase',
                data: JSON.stringify(phrase)
            }).then(function successCallback(response) {
                console.log("deletePhrase SUCCESS: ");
                console.log(response);
                ps.delete(phrase);
                showNotification("Success", "Phrases deleted!", "success");
            }, function errorCallback(response) {
                console.log("POST FAIL:");
                console.log(response);
                showNotification("Error", response.data, "error");
            });
        }


        return api;

    }]);
})();