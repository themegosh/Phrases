(function () {
    "use strict";

    angular.module("MyVoice").factory("ApiService", ['$http', "PhrasesService", function ($http, ps) {

        var api = {};

        api.addPhrase = function (phrase) {
            $http({
                method: 'Post',
                url: '/api/tts/SavePhrase',
                data: JSON.stringify(phrase)
            }).then(function successCallback(response) {
                console.log("savePhrase SUCCESS:");
                console.log(response.data);
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
                console.log(response.data);
                ps.assignAllPhrases(angular.fromJson(response.data));
                showNotification("Success", "Phrases retrieved!", "success");
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

        api.updatePhrase = function (newPhrase, oldPhrase) {

            $http({
                method: 'Post',
                url: '/api/tts/SavePhrase',
                data: JSON.stringify(newPhrase)
            }).then(function successCallback(response) {
                console.log("updatePhrase SUCCESS:");
                console.log(response.data);
                ps.replace(angular.fromJson(response.data), oldPhrase);
                showNotification("Success", "Phrase updated.", "success");
            }, function errorCallback(response) {
                console.log("updatePhrase FAIL:");
                console.log(response);
                showNotification("Error", response.data, "error");
            });
        }

        api.replacePhrase = function (newPhrase, oldPhrase) {
            console.log("NEW:");
            if (newPhrase.Text != oldPhrase.Text) {
                $http({
                    method: 'Post',
                    url: '/api/tts/DeletePhrase',
                    data: JSON.stringify(oldPhrase)
                }).then(function successCallback(response) {
                    $http({
                        method: 'Post',
                        url: '/api/tts/SavePhrase',
                        data: JSON.stringify(newPhrase)
                    }).then(function successCallback(response) {
                        console.log("replacePhrase SUCCESS:");
                        console.log(response.data);
                        ps.replace(angular.fromJson(response.data), oldPhrase);
                        showNotification("Success", "Phrase replaced.", "success");
                    }, function errorCallback(response) {
                        console.log("replacePhrase FAIL:");
                        console.log(response);
                        showNotification("Error", response.data, "error");
                    });
                }, function errorCallback(response) {
                    console.log("POST FAIL:");
                    console.log(response);
                    showNotification("Error", response.data, "error");
                });
            }
            
            
        }


        return api;

    }]);
})();