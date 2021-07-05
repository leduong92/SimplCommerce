
(function (app) {
    
    app.controller('CountryImportController', CountryImportController);

    CountryImportController.$inject = ['$scope'];

    function CountryImportController($scope) {
        var vm = this;

        $scope.message = "Import data from excel file";
    }
})(angular.module('simplAdmin.core'));
