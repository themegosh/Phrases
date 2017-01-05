(function () {
    "use strict";
    var phrasesApp = angular.module("Phrases", [
        'ui.bootstrap',
        'angular-confirm',
        'ng.httpLoader',
        'lr.upload'
    ]);
        
    phrasesApp.config(['httpMethodInterceptorProvider',
        function (httpMethodInterceptorProvider) {
            httpMethodInterceptorProvider.whitelistLocalRequests();
        }
    ]);   


})();