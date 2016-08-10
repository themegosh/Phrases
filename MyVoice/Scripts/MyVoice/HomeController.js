(function () {
    "use strict";
    angular.module("MyVoice").controller("HomeController",
        ["$scope", "$http", '$uibModal', 'PhrasesService', 'angularPlayer', "ApiService",
        function ($scope, $http, $uibModal, ps, angularPlayer, api) {
            
            $scope.newPhrase = {
                Text: "",
                Tags: [],
                Hash: ""
            };

            $scope.allTags = ps.tags;

            $scope.init = function () {
                api.getAllPhrases();
                $scope.phrases = ps.phrases;

            }

            $scope.btnCreateNewPhrase = function () {

                console.log("btnClicked!");
                $scope.newPhrase.Text = $scope.newPhrase.Text.trim();


                $scope.newPhrase.Hash = md5($scope.newPhrase.Text);

                api.addPhrase($scope.newPhrase);
                console.log($scope.newPhrase);

                $scope.newPhrase.Text = "";
            }

            $scope.btnPlay = function (phrase) {

                angularPlayer.addTrack(phrase);
                angularPlayer.playTrack(phrase.id);

                //angularPlayer.clearPlaylist(function () {
                //    angularPlayer.addTrack(phrase);
                //    angularPlayer.playTrack(phrase.id);
                //});
            }

            $scope.delete = function (phrase, $event) {
                $event.stopPropagation();
                console.log("btnDeletePhrase!");
                api.deletePhrase(phrase);
            }

            $scope.btnEditPhrase = function (phrase, $event) {
                $event.stopPropagation();
                console.log("btnEditPhrase!");

                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: '/Scripts/MyVoice/EditPhrase/editPhraseModal.html',
                    controller: 'editPhraseCtrl',
                    size: 'md',
                    resolve: {
                        items: function () {
                            return phrase;
                        }
                    }
                });
            }
            
        }]);
})();