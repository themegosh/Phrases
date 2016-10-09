(function () {
    "use strict";
    UserController.$inject = ['$scope', '$element', '$attrs', '$http'];
    function UserController($scope, $element, $attrs, $http) {
        var $ctrl = this;

        //properties
        
        //events
        $ctrl.$onInit = function () {
            
        }

        $ctrl.btnLogOut = function () {
            $http({
                method: 'POST',
                url: '/Account/LogOff'
            }).then(function successCallback(response) {
                window.location = window.location;
                console.log(response);
            }, function errorCallback(response) {
                console.log(response);
                window.location = window.location;
            });
        }
        
    }

    angular.module("Phrases").component('userComponent', {
        templateUrl: '/Scripts/App/Components/User/UserModal.html',
        controller: UserController
    });
})();