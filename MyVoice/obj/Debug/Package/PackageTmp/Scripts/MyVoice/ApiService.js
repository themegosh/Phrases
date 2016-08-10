(function () {
    "use strict";

    angular.module("MyVoice").factory("ApiService", ['$http', 'PhrasesService', function ($http, PhrasesService) {

        var api = {};

        api.getTTSFile = function (phrase) {
            $http({
                method: 'Post',
                url: '/api/tts/GetFile',
                data: phrase
            }).then(function successCallback(response) {
                console.log("POST SUCCESS:");
                console.log(response);
                PhrasesService.add(angular.fromJson(response.data));
            }, function errorCallback(response) {
                console.log("POST FAIL:");
                console.log(response);
            });
        }


        return api;

    }]);
})();