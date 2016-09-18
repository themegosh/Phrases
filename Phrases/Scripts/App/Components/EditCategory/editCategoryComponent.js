(function () {
    "use strict";

    EditCategoryController.$inject = ['ApiService', 'PhrasesService'];
    function EditCategoryController(api, ps) {
        var $ctrl = this;

        //properties
        $ctrl.modalTitle = "Edit Category";
        $ctrl.category = {};
        $ctrl.isDirty = false;

        //events
        $ctrl.$onInit = function () {
            console.log("$onInit EditPhrasesController");
            if ($ctrl.resolve.category === true) { //new category
                $ctrl.modalTitle = "New Category";
                $ctrl.category = {
                    name: "",
                    icon: ""
                };
            } else {
                angular.copy($ctrl.resolve.category, $ctrl.category);//editing existing
            }
        }

        $ctrl.save = function () {
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