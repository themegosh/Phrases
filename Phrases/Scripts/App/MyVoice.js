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
            url: '',
            component: 'phrases'
        })
        .state('loginPage', {
            url: '/login',
            component: 'login'
        })
        
        
        $urlRouterProvider.otherwise('/');
        
        
    }]);


    function authenticate($q, User, $state, $timeout) {
        if (user.isAuthenticated()) {
            // Resolve the promise successfully
            return $q.when()
        } else {
            // The next bit of code is asynchronously tricky.

            $timeout(function () {
                // This code runs after the authentication promise has been rejected.
                // Go to the log-in page
                $state.go('loginPage')
            })

            // Reject the authentication promise to prevent the state from loading
            return $q.reject()
        }
    }


})();