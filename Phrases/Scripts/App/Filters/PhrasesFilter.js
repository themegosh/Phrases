(function () {
    "use strict";
    angular.module("Phrases")
        .filter('selectedCategory', function () {
            return function (phrases, curCategory) {

                if (curCategory === 'All' || curCategory === '')
                    return phrases;

                var outPhrases = [];

                angular.forEach(phrases, function (phrase) {
                    var shouldAdd = false;
                    angular.forEach(phrase.categories, function (category) {
                        if (category == curCategory) {
                            console.log("adding" + phrase.Text + " category " + curCategory);
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