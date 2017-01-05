(function () {
    "use strict";
    UserController.$inject = ['$scope', '$element', '$attrs', '$http', 'UserService', 'PhrasesService'];
    function UserController($scope, $element, $attrs, $http, us, ps) {
        var $ctrl = this;

        //properties
        $ctrl.user = ps.user;
        
        //events
        $ctrl.$onInit = function () {
            console.log($ctrl);
        }

        $ctrl.btnLogOut = function () {
            $ctrl.dismiss({ $value: 'cancel' });
            us.logout();
        }
        
    }

    angular.module("Phrases").component('userComponent', {
        templateUrl: '/Scripts/App/Components/User/UserModal.html',
        controller: UserController,
        bindings: {
            close: '&',
            dismiss: '&'
        }
    });
})();