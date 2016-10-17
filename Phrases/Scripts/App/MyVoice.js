(function () {
    "use strict";
    var phrasesApp = angular.module("Phrases", [
        'ui.bootstrap',
        'angular-confirm',
        'ng.httpLoader',
        'lr.upload',
        'ui.router'
    ]);
        
    phrasesApp.config(['$stateProvider', '$urlRouterProvider', function($stateProvider, $urlRouterProvider) {
        
        $stateProvider
            .state('phrasesPage', {
                url: '/',
                component: 'phrases',
                resolve: { authenticate: authenticate }
            })
            .state('loginPage', {
                url: '/login',
                component: 'login',
                resolve: { authenticate: authenticate }
            });
        
        $urlRouterProvider.otherwise('/');
        
        function authenticate($q, UserService, $state, $timeout) {
            if (UserService.isAuthenticated()) {
                console.log("authenticate isAuthenticated");
                return $q.when()
            } else {
                $timeout(function () {
                    $state.go('loginPage')
                });
                return $q.reject();
            }
        }
        
    }]);

    


})();