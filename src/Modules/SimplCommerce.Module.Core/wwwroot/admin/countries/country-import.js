(function (app) {
    
    app.controller('CountryImportController', CountryImportController);

    CountryImportController.$inject = ['$scope', '$http', '$state', 'notificationService'];

    function CountryImportController($scope, $http, $state, notificationService) {
        var vm = this;
        $scope.message = "Import data from Excel file";
        $scope.files = [];
        $scope.categoryId = 0;
        $scope.ImportCountries = ImportCountries;
        $scope.flatFolders = [];
        //listen for the file selected event
        $scope.$on("fileSelected", function (event, args) {
            $scope.$apply(function () {
                $scope.files.push(args.file);
            });
        });

        function ImportCountries() {
            $http({
                method: 'POST',
                url: "/api/countries/import",

                headers: { 'Content-Type': undefined },

                transformRequest: function (data) {
                    var formData = new FormData();

                    for (var i = 0; i < data.files.length; i++) {
                        formData.append("file" + i, data.files[i]);
                    }
                    return formData;
                },
                data: { files: $scope.files }
            }).then(function (result, status, headers, config) {
                notificationService.displaySuccess(result.data);
                $state.go('countries');
            },
                function (data, status, headers, config) {
                    notificationService.displayError(data);
                });
        }

        
    }
})(angular.module('simplAdmin.core'));
