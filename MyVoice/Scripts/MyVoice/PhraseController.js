﻿(function () {
    "use strict";
    angular.module("MyVoice").controller("PhraseController",
        ["$scope", "$http", '$uibModal', 'PhrasesService', 'angularPlayer', "ApiService", '$rootScope',
        function ($scope, $http, $uibModal, ps, angularPlayer, api, $rootScope) {
            
            $scope.newPhrase = {
                Text: "",
                Tags: [],
                Hash: ""
            };

            $scope.slider = {
                curVol: 99,
                options: {
                    floor: 0,
                    ceil: 100,
                    translate: function (value) {
                        return value + '%';
                    },
                    onChange: function () {
                        console.log("changeVol: " + $scope.slider.curVol);
                        angularPlayer.changeVolume($scope.slider.curVol);
                    }
                }
            };

            $scope.something = 1;

            $scope.allTags = ps.tags;

            $scope.init = function () {
                api.getAllPhrases();
                $scope.phrases = ps.phrases;
                $scope.tagFilter = ps.tagFilter;

            }

            $scope.changeFilter = function (tag) {
                //showNotification("DEBUG", "Changing tag: " + tag, "info");
                ps.tagFilter = angular.copy(tag);
                $scope.tagFilter = ps.tagFilter;
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

            $scope.btnDeletePhrase = function (phrase, $event) {
                console.log("btnDeletePhrase!");
                api.deletePhrase(phrase);
            }

            $scope.btnEditPhrase = function (phrase, $event) {
                console.log("btnEditPhrase!");
                $event.stopPropagation();

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

            $scope.btnChangeVolume = function () {
                console.log("btnChangeVolume!");

                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: '/Scripts/MyVoice/Volume/VolumeModal.html',
                    controller: 'volumeCtrl',
                    size: 'md'
                });
                modalInstance.rendered.then(function () {
                    $rootScope.$broadcast('rzSliderForceRender'); //Force refresh sliders on render. Otherwise bullets are aligned at left side.
                });
            }
                        
        }]);
})();