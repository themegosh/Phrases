(function () {
    "use strict";

    LoginController.$inject = ['$scope', 'ApiService', 'PhrasesService', '$uibModal'];
    function LoginController($scope, api, ps, $uibModal) {
        var $ctrl = this;

        //properties
        $ctrl.editMode = false;
        $ctrl.categoryFilter = {
            name: "All"
        };
        
        //events
        $ctrl.$onInit = function () {
            console.log("")
            api.login({
                "Email":"mathewdf@gmail.com",
                "Password":"jamaca23",
                "RememberMe":true
            });
        }

    }

    angular.module("Phrases").component('login', {
        templateUrl: '/Scripts/App/Components/Login/Login.html',
        bindings: { },
        controller: LoginController
    });
})();