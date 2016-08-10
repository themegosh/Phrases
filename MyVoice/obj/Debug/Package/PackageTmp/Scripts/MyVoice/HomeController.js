(function () {
    "use strict";
    angular.module("MyVoice").controller("HomeController",
        ["$scope", "$http", '$uibModal', 'ApiService', 'PhrasesService', 'angularPlayer',
        function ($scope, $http, $uibModal, apiS, ps, angularPlayer) {
            
            $scope.newPhrase = {
                Text: "",
                Tags: "",
                FileName: ""
            };

            $scope.init = function () {

                console.log("start");

                $scope.phrases = ps.phrases;
            }

            $scope.btnTalk = function () {

                console.log("btnClicked!");

                apiS.getTTSFile($scope.newPhrase);
                console.log($scope.newPhrase);
            }

            $scope.btnPlay = function (phrase) {

                angularPlayer.clearPlaylist(function () {
                    angularPlayer.addTrack(phrase);
                    angularPlayer.playTrack(phrase.id);
                });
            }

        }]);
})();