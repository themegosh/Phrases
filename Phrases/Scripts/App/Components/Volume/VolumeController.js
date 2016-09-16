(function () {
    "use strict";
    angular.module("MyVoice").controller("volumeCtrl",
        ["$scope", "$uibModalInstance", 'PhrasesService', 'angularPlayer', '$rootScope',
        function ($scope, $uibModalInstance, ps, angularPlayer, $rootScope) {

            $scope.slider = {
                curVol: angularPlayer.getVolume(),
                options: {
                    floor: 0,
                    ceil: 100,
                    translate: function (value) {
                        return value + '%';
                    },
                    onChange: function () {
                        //console.log("changeVol: " + $scope.slider.curVol);
                        angularPlayer.changeVolume($scope.slider.curVol);
                    }
                }
            };


            //$scope.init = function () {
            //    console.log("init");
                
            //    $rootScope.$broadcast('rzSliderForceRender'); //fix slider after modal init

            //}

            $scope.ok = function () {
                $uibModalInstance.dismiss('cancel');
            }

        }]);
})();