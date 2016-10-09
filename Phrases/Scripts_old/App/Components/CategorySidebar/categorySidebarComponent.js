(function () {
    "use strict";
    CategorySidebarController.$inject = ['$uibModal'];
    function CategorySidebarController($uibModal) {
        var $ctrl = this;

        //properties

        //events
        $ctrl.$onInit = function () {
        }

        $ctrl.btnSelectFilter = function (category) {
            if (!$ctrl.editMode) {
                if (category.name === 'All')
                    $ctrl.categoryFilter = {
                        name: "All"
                    };
                else
                    angular.copy(category, $ctrl.categoryFilter);
            } else {
                if (category.name === 'All') //cant edit "all"
                    return;

                //open category editor
                var modalInstance = $uibModal.open({
                    animation: true,
                    component: 'editCategoryComponent',
                    size: 'md',
                    resolve: {
                        category: function () {
                            return category;
                        }
                    }
                });
            }
        }

    }

    angular.module("Phrases").component('categorySidebar', {
        templateUrl: '/Scripts/App/Components/CategorySidebar/categorySidebar.html',
        bindings: {
            categoryFilter: '=',
            categories: '<',
            editMode: '<'
        },
        controller: CategorySidebarController
    });
})();