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
        
        .state('phrasesState', {
            url: '',
            component: 'phrases'
        })
        .state('loginState', {
            url: '/login',
            component: 'login'
        })
        
        
        $urlRouterProvider.otherwise('/');
        
        
    }]);

    phrasesApp.run(function($rootScope, PhrasesService, $state) {
        // Listen to '$locationChangeSuccess', not '$stateChangeStart'
        $rootScope.$on('$locationChangeSuccess', function() {
            if (angular.isUndefined(PhrasesService.user.User)) {
                $state.go('loginState');
            }
        })
    });


})();