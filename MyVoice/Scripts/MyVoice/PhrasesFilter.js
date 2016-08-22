(function () {
    "use strict";
    angular.module("MyVoice")
        .filter('selectedTag', function () {
            return function (phrases, curTag) {

                if (curTag === 'All')
                    return phrases;

                var outPhrases = [];

                angular.forEach(phrases, function (phrase) {
                    var shouldAdd = false;
                    angular.forEach(phrase.tags, function (tag) {
                        if (tag == curTag) {
                            //console.log("adding" + phrase.Text + " tag " + curTag);
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