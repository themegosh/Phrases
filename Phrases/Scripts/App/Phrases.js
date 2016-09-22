(function () {
    "use strict";
    angular.module("Phrases", [
        "ui.bootstrap",
        'angularSoundManager',
        'frapontillo.bootstrap-switch',
        'rzModule',
        'ngTouch',
        'angular-confirm',
        'as.sortable',
        'ng.httpLoader'
    ])
    .config(['httpMethodInterceptorProvider',
        function (httpMethodInterceptorProvider) {
            httpMethodInterceptorProvider.whitelistLocalRequests();
        }
    ]);

})();