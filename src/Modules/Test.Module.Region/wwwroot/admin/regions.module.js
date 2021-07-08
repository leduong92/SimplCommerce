/// <reference path="../../angular/angular.js" />
(function () {
    'use strict';

    angular
        .module('test.region', [])
        .config(['$stateProvider', function ($stateProvider) {
            $stateProvider.state('regions', {
                url: '/regions',
                templateUrl: "_content/Test.Module.Region/admin/regions/region-list.html",
                controller: 'regionListController as vm'
            });

        }]);
   
})();
