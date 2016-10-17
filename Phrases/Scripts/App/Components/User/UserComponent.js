(function () {
    "use strict";
    UserController.$inject = ['$scope', '$element', '$attrs', '$http', 'UserService'];
    function UserController($scope, $element, $attrs, $http, us) {
        var $ctrl = this;

        //properties
        
        //events
        $ctrl.$onInit = function () {
            
        }

        $ctrl.btnLogOut = function () {
            us.logout();
        }
        
    }

    angular.module("Phrases").component('userComponent', {
        templateUrl: '/Scripts/App/Components/User/UserModal.html',
        controller: UserController
    });
})();