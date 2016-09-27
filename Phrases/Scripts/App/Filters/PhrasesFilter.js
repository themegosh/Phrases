(function () {
    "use strict";
    angular.module("Phrases")
        .filter('selectedCategory', function () {
            return function (phrases, curCategory) {
                
                if (curCategory.name === 'All' || curCategory.name === '')
                    return phrases;

                var outPhrases = [];

                angular.forEach(phrases, function (phrase) {
                    var shouldAdd = false;
                    angular.forEach(phrase.categories, function (category) {
                        if (category == curCategory.guid) {
                            shouldAdd = true;
                        }
                    });
                    if (shouldAdd) {
                        outPhrases.push(phrase);
                    }
                });

                return outPhrases;
            };
        });
})();