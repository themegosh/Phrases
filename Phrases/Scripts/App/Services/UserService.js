(function () {
    "use strict";

    angular.module("Phrases").factory("UserService", ['$http', "PhrasesService", 'SoundService', 'ApiService', '$state', function ($http, ps, ss, api, $state) {
        var user = {};
        var base_url = "http://localhost:61678";
        
        //{"Email":"","Password":"","RememberMe":true}
        user.login = function (loginData) {
            $http({
                method: 'POST',
                url: base_url + '/api/account/Login',
                headers: { 'Content-Type': "application/json" },
                data: angular.toJson(loginData, false),
            }).then(function successCallback(response) {
                console.log("login SUCCESS:");
                console.log(response);
                user.identity = { response };
                $state.go("phrasesPage");
                //ps.savePhrase(angular.fromJson(response.data));
                showNotification("Success", "Logged In.", "success");
            }, function errorCallback(response) {
                console.log("login FAIL:");
                console.log(response.data);
                showNotification("Error", response.data, "error");
            });
        }
        
        user.logout = function () {
            delete user.identity;
            $http({
                method: 'POST',
                url: base_url + '/api/account/Logoff'
            }).then(function successCallback(response) {
                console.log("Logout SUCCESS:");
                console.log(response);
                //ps.savePhrase(angular.fromJson(response.data));
                showNotification("Success", "Logged out.", "success");
                $state.go('loginPage')
            }, function errorCallback(response) {
                console.log("Logout FAIL:");
                console.log(response.data);
                showNotification("Logout Error", response.data, "error");
            });
        }

        user.restoreIdentity = function () {
            $http({
                method: 'POST',
                url: base_url + '/api/account/GetLoginStatus'
            }).then(function successCallback(response) {
                console.log("GetLoginStatus SUCCESS:");
                console.log(response);
                user.identity = response.data;
                //ps.savePhrase(angular.fromJson(response.data));
                showNotification("Success", "Resumed session...", "success");
                console.log("isAuthenticated: " + true);
                $state.go("phrasesPage");
            }, function errorCallback(response) {
                console.log("GetLoginStatus FAIL:");
                console.log(response.data);
                console.log("isAuthenticated: " + false);
            });
        }
        
        user.isAuthenticated = function (shouldRestore) {
            if (angular.isDefined(user.identity)) {
                console.log("isAuthenticated: " + true);
                return true;
            } else {
                if (shouldRestore) {
                    user.restoreIdentity();
                }
                return false;
            }
        }

        return user;

    }]);
})();