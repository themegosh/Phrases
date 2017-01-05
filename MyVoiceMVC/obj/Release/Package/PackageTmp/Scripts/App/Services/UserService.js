(function () {
    "use strict";

    angular.module("Phrases").factory("UserService", ['$http', "PhrasesService", 'SoundService', 'ApiService', function ($http, ps, ss, api) {
        var user = {};
        
        //{"Email":"","Password":"","RememberMe":true}
        //user.login = function (loginData) {
        //    console.log(loginData);
        //    loginData.grant_type = "password";


        //    var data = "grant_type=password&username=" + loginData.username + "&password=" + loginData.password;

        //    $http.post(base_url + '/oauth/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

        //        localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName });

        //        _authentication.isAuth = true;
        //        _authentication.userName = loginData.userName;


        //        console.log("Success " + response);

        //    }).error(function (err, status) {
        //        console.log("Error " + status);
        //        console.log(err);
        //    });



        //    //$http({
        //    //    method: 'POST',
        //    //    url: base_url + '/oauth/token',
        //    //    headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8', },
        //    //    data: angular.toJson(loginData, false),
        //    //}).then(function successCallback(response) {
        //    //    console.log("login SUCCESS:");
        //    //    console.log(response);
        //    //    user.identity = { response };
        //    //    //ps.savePhrase(angular.fromJson(response.data));
        //    //    showNotification("Success", "Logged In.", "success");
        //    //}, function errorCallback(response) {
        //    //    console.log("login FAIL:");
        //    //    console.log(response.data);
        //    //    showNotification("Error", response.data, "error");
        //    //});
        //}
        
        user.logout = function () {
            delete user.identity;
            $http({
                method: 'POST',
                url: '/account/Logoff'
            }).then(function successCallback(response) {
                console.log("Logout SUCCESS:");
                console.log(response);
                //ps.savePhrase(angular.fromJson(response.data));
                showNotification("Success", "Logged out.", "success");
                location.reload();
            }, function errorCallback(response) {
                console.log("Logout FAIL:");
                console.log(response.data);
                showNotification("Logout Error", response.data, "error");
            });
        }

        //user.restoreIdentity = function () {
        //    $http({
        //        method: 'POST',
        //        url: base_url + '/api/account/GetLoginStatus'
        //    }).then(function successCallback(response) {
        //        console.log("GetLoginStatus SUCCESS:");
        //        console.log(response);
        //        user.identity = response.data;
        //        //ps.savePhrase(angular.fromJson(response.data));
        //        showNotification("Success", "Resumed session...", "success");
        //        console.log("isAuthenticated: " + true);
        //    }, function errorCallback(response) {
        //        console.log("GetLoginStatus FAIL:");
        //        console.log(response.data);
        //        console.log("isAuthenticated: " + false);
        //    });
        //}
        
        //user.isAuthenticated = function (shouldRestore) {
        //    if (angular.isDefined(user.identity)) {
        //        console.log("isAuthenticated: " + true);
        //        return true;
        //    } else {
        //        if (shouldRestore) {
        //            user.restoreIdentity();
        //        }
        //        return false;
        //    }
        //}

        return user;

    }]);
})();