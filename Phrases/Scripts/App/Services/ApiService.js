(function () {
    "use strict";

    angular.module("Phrases").factory("ApiService", ['$http', "PhrasesService", 'SoundService', '$state', function ($http, ps, ss, $state) {

        var api = {};
        var base_url = "http://localhost:61678";

        api.getUserData = function () {
            $http({
                method: 'Get',
                url: base_url + '/api/tts/getUserData'
            }).then(function successCallback(response) {
                console.log("getAllPhrases SUCCESS: ");
                console.log(response);
                ps.importUserData(angular.fromJson(response.data));
            }, function errorCallback(response) {
                console.log("getAllPhrases FAIL:");
                console.log(response);
                showNotification("Error", response.data, "error");
            });
        }

        api.savePhrase = function (phrase) {
            $http({
                method: 'POST',
                url: base_url + '/api/tts/SavePhrase',
                headers: { 'Content-Type': "application/json" },
                data: angular.toJson(phrase, false),
            }).then(function successCallback(response) {
                console.log("savePhrase SUCCESS:");
                console.log(response.data);
                ps.savePhrase(angular.fromJson(response.data));
                showNotification("Success", "Phrase saved.", "success");
            }, function errorCallback(response) {
                console.log("savePhrase FAIL:");
                console.log(response.data);
                showNotification("Error", response.data, "error");
            });
        }
        
        //{"Email":"mathewdf@gmail.com","Password":"jamaca23","RememberMe":true}
        api.login = function (loginData) {
            $http({
                method: 'POST',
                url: base_url + '/account/Login',
                headers: { 'Content-Type': "application/json" },
                data: angular.toJson(loginData, false),
            }).then(function successCallback(response) {
                console.log("login SUCCESS:");
                console.log(response);
                $state.go("phrasesPage");
                //ps.savePhrase(angular.fromJson(response.data));
                showNotification("Success", "Logged In.", "success");
            }, function errorCallback(response) {
                console.log("login FAIL:");
                console.log(response.data);
                showNotification("Error", response.data, "error");
            });
        }
        
        api.quickPhrase = function (phrase) {
            $http({
                method: 'POST',
                url: base_url + '/api/tts/QuickPhrase',
                headers: { 'Content-Type': "application/json" },
                data: angular.toJson(phrase, false),
            }).then(function successCallback(response) {
                console.log("quickPhrase SUCCESS:");
                angular.copy(angular.fromJson(response.data), phrase);
                ss.playSound(phrase.tempGuid, true);
                console.log(phrase);
            }, function errorCallback(response) {
                console.log("quickPhrase FAIL:");
                console.log(response.data);
                showNotification("Error", response.data, "error");
            });
        }

        api.deletePhrase = function (phrase) {
            $http({
                method: 'Post',
                url: base_url + '/api/tts/DeletePhrase',
                data: JSON.stringify(phrase)
            }).then(function successCallback(response) {
                console.log("deletePhrase SUCCESS: ");
                console.log(response);
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
                url: base_url + '/api/tts/SaveCategory',
                data: JSON.stringify(category)
            }).then(function successCallback(response) {
                console.log("saveCategory SUCCESS: ");
                console.log(response);
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
                url: base_url + '/api/tts/DeleteCategory',
                data: JSON.stringify(category)
            }).then(function successCallback(response) {
                console.log("deleteCategory SUCCESS: ");
                console.log(response);
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