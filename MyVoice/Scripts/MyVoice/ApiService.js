(function () {
    "use strict";

    angular.module("MyVoice").factory("ApiService", ['$http', "PhrasesService", function ($http, ps) {

        var api = {};

        api.savePhrase = function (phrase) {
            $.ajax({
                url: "/api/tts/SavePhrase",
                data: angular.toJson(phrase, false),
                contentType: "application/json",
                type: 'POST',
                beforeSend: function (xhr, opts) {
                    console.log("savePhrase() beforeSend");
                    //xhr.abort();
                },
                success: function (response) {
                    console.log("savePhrase SUCCESS:");
                    console.log(response);
                    ps.save(angular.fromJson(response));
                    showNotification("Success", "Phrase saved.", "success");
                },
                error: function (response) {
                    console.log("savePhrase FAIL:");
                    console.log(response);
                    showNotification("Error", response.data, "error");
                },
                complete: function () {
                    console.log("savePhrase() complete");
                }
            });
        }

        api.getAllPhrases = function () {
            $http({
                method: 'Get',
                url: '/api/tts/GetAllPhrases'
            }).then(function successCallback(response) {
                console.log("getAllPhrases SUCCESS: ");
                console.log(response.data);
                ps.assignAllPhrases(angular.fromJson(response.data));
                //showNotification("Success", "Phrases retrieved!", "success");
            }, function errorCallback(response) {
                console.log("getAllPhrases FAIL:");
                console.log(response);
                showNotification("Error", response.data, "error");
            });
        }

        api.deletePhrase = function (phrase) {
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