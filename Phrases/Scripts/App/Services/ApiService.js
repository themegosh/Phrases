(function () {
    "use strict";

    angular.module("Phrases").factory("ApiService", ['$http', "PhrasesService", function ($http, ps) {

        var api = {};

        api.savePhrase = function (phrase) {
            $http({
                method: 'POST',
                url: '/api/tts/SavePhrase',
                headers: { 'Content-Type': "application/json" },
                data: angular.toJson(phrase, false),
            }).then(function successCallback(response) {
                console.log("savePhrase SUCCESS:");
                console.log(response.data);
                ps.save(angular.fromJson(response.data));
                showNotification("Success", "Phrase saved.", "success");
            }, function errorCallback(response) {
                console.log("savePhrase FAIL:");
                console.log(response.data);
                showNotification("Error", response.data, "error");
            });
        }

        //api.getAllPhrases = function () {
        //    $http({
        //        method: 'Get',
        //        url: '/api/tts/GetAllPhrases'
        //    }).then(function successCallback(response) {
        //        console.log("getAllPhrases SUCCESS: ");
        //        console.log(response.data);
        //        ps.importPhrases(angular.fromJson(response.data));
        //        //showNotification("Success", "Phrases retrieved!", "success");
        //    }, function errorCallback(response) {
        //        console.log("getAllPhrases FAIL:");
        //        console.log(response);
        //        showNotification("Error", response.data, "error");
        //    });
        //}

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

        api.getUserData = function () {
            $http({
                method: 'Get',
                url: '/api/tts/getUserData'
            }).then(function successCallback(response) {
                console.log("getAllPhrases SUCCESS: ");
                console.log(response.data);
                ps.importPhrases(angular.fromJson(response.data));
                //showNotification("Success", "Phrases retrieved!", "success");
            }, function errorCallback(response) {
                console.log("getAllPhrases FAIL:");
                console.log(response);
                showNotification("Error", response.data, "error");
            });
        }

        return api;

    }]);
})();