(function () {
    "use strict";

    EditCategoryController.$inject = ['ApiService', 'PhrasesService', '$confirm'];
    function EditCategoryController(api, ps, $confirm) {
        var $ctrl = this;

        $ctrl.icons = ['icon-user', 'icon-people', 'icon-user-female', 'icon-emotsmile', 'icon-phone', 'icon-map', 'icon-location-pin', 'icon-direction', 'icon-directions', 'icon-compass', 'icon-check', 'icon-clock', 'icon-event', 'icon-exclamation', 'icon-organization', 'icon-trophy', 'icon-screen-smartphone', 'icon-screen-desktop', 'icon-plane', 'icon-notebook', 'icon-mustache', 'icon-mouse', 'icon-magnet', 'icon-energy', 'icon-disc', 'icon-chemistry', 'icon-speedometer', 'icon-shield', 'icon-screen-tablet', 'icon-magic-wand', 'icon-hourglass', 'icon-graduation', 'icon-ghost', 'icon-game-controller', 'icon-fire', 'icon-eyeglass', 'icon-envelope-letter', 'icon-bell', 'icon-badge', 'icon-anchor', 'icon-wallet', 'icon-speech', 'icon-puzzle', 'icon-printer', 'icon-present', 'icon-playlist', 'icon-pin', 'icon-picture', 'icon-handbag', 'icon-globe-alt', 'icon-globe', 'icon-film', 'icon-feed', 'icon-drop', 'icon-diamond', 'icon-cup', 'icon-calculator', 'icon-bubbles', 'icon-briefcase', 'icon-book-open', 'icon-basket-loaded', 'icon-basket', 'icon-wrench', 'icon-umbrella', 'icon-trash', 'icon-tag', 'icon-support', 'icon-rocket', 'icon-question', 'icon-pie-chart', 'icon-pencil', 'icon-note', 'icon-loop', 'icon-home', 'icon-microphone', 'icon-music-tone-alt', 'icon-music-tone', 'icon-earphones-alt', 'icon-earphones', 'icon-like', 'icon-dislike', 'icon-volume-2', 'icon-volume-off', 'icon-calendar', 'icon-bulb', 'icon-chart', 'icon-ban', 'icon-bubble', 'icon-camrecorder', 'icon-camera', 'icon-cloud-download', 'icon-cloud-upload', 'icon-envelope', 'icon-eye', 'icon-flag', 'icon-heart', 'icon-info', 'icon-key', 'icon-link', 'icon-lock', 'icon-lock-open', 'icon-magnifier', 'icon-magnifier-add', 'icon-magnifier-remove', 'icon-paper-clip', 'icon-paper-plane', 'icon-power', 'icon-refresh', 'icon-reload', 'icon-settings', 'icon-star', 'icon-symbol-female', 'icon-symbol-male', 'icon-target'];

        //properties
        $ctrl.modalTitle = "Edit Category";
        $ctrl.category = {};
        $ctrl.isDirty = false;

        //events
        $ctrl.$onInit = function () {
            if ($ctrl.resolve.category == null) { //new category
                $ctrl.modalTitle = "New Category";
                $ctrl.category = {
                    name: "",
                    icon: ""
                };
                $('#txtCategoryName').ready(function () {
                    $('#txtCategoryName').focus();
                    $('#txtCategoryName').click();
                });
            } else {
                angular.copy($ctrl.resolve.category, $ctrl.category);//editing existing
            }
        }

        $ctrl.save = function () {
            $ctrl.isDirty = false;
            if ($ctrl.category.name != "") {
                api.saveCategory($ctrl.category);
            }
            $ctrl.dismiss({ $value: 'cancel' });
        }

        $ctrl.cancel = function () {
            console.log($ctrl.isDirty);
            if ($ctrl.isDirty) {
                $confirm({ text: 'Are you sure you want to close without saving this category?' })
                    .then(function () {
                        $ctrl.dismiss({ $value: 'cancel' });
                    });
            } else {
                $ctrl.dismiss({ $value: 'cancel' });
            }
        }

        $ctrl.delete = function () {
            api.deleteCategory($ctrl.resolve.category);
            $ctrl.dismiss({ $value: 'cancel' });
        }

        $ctrl.changeIcon = function (icon) {
            $ctrl.isDirty = true;
            $ctrl.category.icon = icon;
        }
    }

    angular.module("Phrases").component('editCategoryComponent', {
        templateUrl: '/Scripts/App/Components/EditCategory/editCategoryModal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: EditCategoryController
    });
})();