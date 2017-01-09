(function () {
    "use strict";

    angular.module("Phrases").factory("ApiService", ['$http', "PhrasesService", 'SoundService', function ($http, ps, ss) {

        var api = {};

        api.getUserData = function () {
            $http({
                method: 'Get',
                url: '/tts/getUserData'
            }).then(function successCallback(response) {
                //console.log("getAllPhrases SUCCESS: ");
                //console.log(response);
                ps.processUserData(angular.fromJson(response.data));
            }, function errorCallback(response) {
                console.log("getAllPhrases FAIL:");
                console.log(response);
                showNotification("Error", response.data, "error");
            });
        }

        api.savePhrase = function (phrase) {
            $http({
                method: 'POST',
                url: '/tts/SavePhrase',
                headers: { 'Content-Type': "application/json" },
                data: angular.toJson(phrase, false),
            }).then(function successCallback(response) {
                //console.log("savePhrase SUCCESS:");
                //console.log(response.data);
                ps.savePhrase(angular.fromJson(response.data));
                showNotification("Success", "Phrase saved.", "success");
            }, function errorCallback(response) {
                console.log("savePhrase FAIL:");
                console.log(response.data);
                showNotification("Error", response.data, "error");
            });
        }
                
        api.quickPhrase = function (phrase) {
            var tempPhrase = angular.copy(phrase);
            $http({
                method: 'POST',
                url: '/tts/QuickPhrase',
                headers: { 'Content-Type': "application/json" },
                data: angular.toJson(tempPhrase, false),
            }).then(function successCallback(response) {
                //console.log("quickPhrase SUCCESS:");
                angular.copy(angular.fromJson(response.data), tempPhrase);
                ss.playSound(tempPhrase, true);
                //console.log(tempPhrase);
            }, function errorCallback(response) {
                console.log("quickPhrase FAIL:");
                console.log(response.data);
                showNotification("Error", response.data, "error");
            });
        }

        api.deletePhrase = function (phrase) {
            $http({
                method: 'Post',
                url: '/tts/DeletePhrase',
                data: JSON.stringify(phrase)
            }).then(function successCallback(response) {
                //console.log("deletePhrase SUCCESS: ");
                //console.log(response);
                ps.deletePhrase(phrase);
                showNotification("Success", "Phrases deleted!", "success");
            }, function errorCallback(response) {
                console.log("deletePhrase FAIL:");
                console.log(response);
                showNotification("Error", response.data, "error");
            });
        }

        api.saveCategory = function (category) {
            $http({
                method: 'Post',
                url: '/tts/SaveCategory',
                data: JSON.stringify(category)
            }).then(function successCallback(response) {
                //console.log("saveCategory SUCCESS: ");
                //console.log(response);
                ps.saveCategory(angular.fromJson(response.data));
                showNotification("Success", "Category saved!", "success");
            }, function errorCallback(response) {
                console.log("saveCategory FAIL:");
                console.log(response);
                showNotification("Error", response.data, "error");
            });
        }
        
        api.deleteCategory = function (category) {
            $http({
                method: 'Post',
                url: '/tts/DeleteCategory',
                data: JSON.stringify(category)
            }).then(function successCallback(response) {
                //console.log("deleteCategory SUCCESS: ");
                //console.log(response);
                ps.deleteCategory(category);
                showNotification("Success", "Category deleted!", "success");
            }, function errorCallback(response) {
                console.log("deleteCategory FAIL:");
                console.log(response);
                showNotification("Error", response.data, "error");
            });
        }        

        return api;

    }]);
})();