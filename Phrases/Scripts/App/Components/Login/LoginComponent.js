(function () {
    "use strict";

    LoginController.$inject = ['$scope', 'ApiService', 'PhrasesService', 'UserService', '$uibModal'];
    function LoginController($scope, api, ps, us, $uibModal) {
        var $ctrl = this;
        $ctrl.user = {};

        //properties
        
        //events
        $ctrl.$onInit = function () {

        }

        $ctrl.btnLogin = function () {
            //{
            //    "Email": "mathewdf@gmail.com",
            //    "Password": "jamaca23",
            //    "RememberMe": true
            //}
            console.log($ctrl.user);
            us.login($ctrl.user);
        }

    }

    angular.module("Phrases").component('login', {
        templateUrl: '/Scripts/App/Components/Login/Login.html',
        bindings: { },
        controller: LoginController
    });
})();