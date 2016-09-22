(function () {
    "use strict";
    angular.module("Phrases").factory("PhrasesService", [ function () {

        var ps = {};

        ps.phrases = [];
        ps.phraseIndex = 1; //user for soundManager's id system
        ps.categories = [];
        ps.user = {};

        ps.importUserData = function (userData) {

            //user
            ps.user = userData.User;
            //categories
            ps.categories.length = 0;
            angular.copy(userData.Categories, ps.categories);
            //phrases
            ps.phrases.length = 0;//clear the old ones
            ps.phraseIndex = 1; //cant start at 0 (soundmanager req)
            angular.forEach(userData.Phrases, function (phrase) {
                ps.addSoundManagerProperties(phrase);
                ps.phrases.push(phrase);
            });

            console.log("ps.importUserData()");
            console.log(ps);
        }
        
        ps.savePhrase = function (phrase) {
            ps.addSoundManagerProperties(phrase);
            var shouldAdd = true;
            angular.forEach(ps.phrases, function (aPhrase, key) {
                if (aPhrase.guid == phrase.guid) { //update this one instead of adding
                    shouldAdd = false;
                    ps.phrases[key] = angular.copy(phrase);
                    console.log("ps.save() updating phrase");
                }
            });
            if (shouldAdd === true) {
                console.log("ps.savePhrase() saving new phrase");
                ps.phrases.push(phrase);
            }
        }
        
        ps.deletePhrase = function (phrase) {
            console.log("ps.deletePhrase() deleting phrase");
            angular.forEach(ps.phrases, function (aPhrase, key) {
                if (aPhrase.text === phrase.text) {
                    ps.phrases.splice(key, 1); //remove this one
                }
            });
        }

        ps.saveCategory = function (category) {
            var shouldAdd = true;
            angular.forEach(ps.categories, function (aCategory, key) {
                if (aCategory.guid == category.guid) { //update this one instead of adding
                    shouldAdd = false;
                    ps.categories[key] = angular.copy(category);
                    console.log("ps.saveCategory() updating category");
                }
            });
            if (shouldAdd === true) {
                ps.categories.push(category);
                console.log("ps.saveCategory() saving new category");
            }
        }

        ps.deleteCategory = function (category) {
            console.log("ps.deleteCategory() deleting category");
            angular.forEach(ps.categories, function (aCategory, key) {
                if (aCategory.name === category.name) {
                    ps.categories.splice(key, 1); //remove this one
                }
            });
        }

        //ps.replace = function (newPhrase, oldPhrase) {
        //    angular.forEach(ps.phrases, function (aPhrase, key, phrases) {
        //        if (aPhrase.text === oldPhrase.text) { //found it
        //            //replace it
        //            newPhrase.id = ps.phraseIndex;
        //            ps.phraseIndex++;
        //            ps.phrases[key] = newPhrase;
        //        }
        //    });
        //}
     
        //private functions
        //ps.refreshTags = function () {
        //    ps.tags.length = 0;
        //    angular.forEach(ps.phrases, function (aPhrase, key) {
        //        angular.forEach(aPhrase.tags, function (tag) {
        //            if ($.inArray(tag, ps.tags) == -1) {
        //                ps.tags.push(tag);
        //            }
        //        });
        //    });
        //}

        ps.addSoundManagerProperties = function (phrase) {
            //required attributes for angular sound-manager
            phrase.id = ps.phraseIndex;
            ps.phraseIndex++;
            phrase.title = phrase.text;
            phrase.artist = "";
            phrase.url = "api/tts/GetAudio?id=" + phrase.guid + ".mp3";
        }

        return ps;

    }]);
})();