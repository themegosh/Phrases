(function () {
    "use strict";
    angular.module("Phrases", [
        'ui.bootstrap',
        'angular-confirm',
        'ng.httpLoader',
        'lr.upload'
    ])
    .config(['httpMethodInterceptorProvider',
        function (httpMethodInterceptorProvider) {
            httpMethodInterceptorProvider.whitelistLocalRequests();
        }
    ]);

})();