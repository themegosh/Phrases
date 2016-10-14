(function () {
    "use strict";

    angular.module("Phrases").factory("User", ['$http', "PhrasesService", 'SoundService', function ($http, ps, ss) {

        var user = {};
        var base_url = "http://localhost:61678";


        //{"Email":"mathewdf@gmail.com","Password":"jamaca23","RememberMe":true}
        user.login = function (loginData) {
            $http({
                method: 'POST',
                url: base_url + '/account/Login',
                headers: { 'Content-Type': "application/json" },
                data: angular.toJson(loginData, false),
            }).then(function successCallback(response) {
                console.log("login SUCCESS:");
                console.log(response.data);
                //ps.savePhrase(angular.fromJson(response.data));
                showNotification("Success", "Logged In.", "success");
            }, function errorCallback(response) {
                console.log("login FAIL:");
                console.log(response.data);
                showNotification("Error", response.data, "error");
            });
        } 
        
        user.isAuthenticated = function (){
            if (angular.isDefined(user.data)){
                return true;
            } else {
                return false;
            }
        }

        return user;

    }]);
})();