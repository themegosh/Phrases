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
                //ps.addSoundManagerProperties(phrase);
                ps.phrases.push(phrase);
            });

            ps.resizeScrollbar();

            //console.log("ps.importUserData()");
            //console.log(ps);
        }

        ps.resizeScrollbar = function () {
            //console.log("Resizing scrollbar");
            var scrollbarMaxHeight = $('.page-sidebar-menu').outerHeight();
            $('.page-sidebar').height($(window).height() - $('.page-header').outerHeight());
        }
        
        ps.savePhrase = function (phrase) {
            //ps.addSoundManagerProperties(phrase);
            var shouldAdd = true;
            angular.forEach(ps.phrases, function (aPhrase, key) {
                if (aPhrase.guid == phrase.guid) { //update this one instead of adding
                    shouldAdd = false;
                    ps.phrases[key] = angular.copy(phrase);
                    //console.log("ps.save() updating phrase");
                }
            });
            if (shouldAdd === true) {
                //console.log("ps.savePhrase() saving new phrase");
                ps.phrases.push(phrase);
            }
            ps.resizeScrollbar();
        }
        
        ps.deletePhrase = function (phrase) {
            //console.log("ps.deletePhrase() deleting phrase");
            angular.forEach(ps.phrases, function (aPhrase, key) {
                if (aPhrase.text === phrase.text) {
                    ps.phrases.splice(key, 1); //remove this one
                }
            });
            ps.resizeScrollbar();
        }

        ps.saveCategory = function (category) {
            var shouldAdd = true;
            angular.forEach(ps.categories, function (aCategory, key) {
                if (aCategory.guid == category.guid) { //update this one instead of adding
                    shouldAdd = false;
                    ps.categories[key] = angular.copy(category);
                    //console.log("ps.saveCategory() updating category");
                }
            });
            if (shouldAdd === true) {
                ps.categories.push(category);
                //console.log("ps.saveCategory() saving new category");
            }
            ps.resizeScrollbar();
        }

        ps.deleteCategory = function (category) {
            //console.log("ps.deleteCategory() deleting category");
            angular.forEach(ps.categories, function (aCategory, key) {
                if (aCategory.name === category.name) {
                    ps.categories.splice(key, 1); //remove this one
                }
            });
            ps.resizeScrollbar();
        }

        return ps;

    }]);
})();