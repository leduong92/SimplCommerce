/*global angular*/
(function () {
   var common = angular
        .module('simplAdmin.common', []);

   common.directive('decimal', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelController) {
                function transform(text) {
                    if (text) {
                        var val = text.replace(/[^0-9.,]/g, '');
                        if (val !== text) {
                            ngModelController.$setViewValue(val);
                            ngModelController.$render();
                        }

                        return val.replace(",", ".");
                    }

                    return null;
                }
                ngModelController.$parsers.push(transform);
            }
        };
   });

    common.directive('fileUpload', function () {
        return {
            scope: true,        //create a new scope
            link: function (scope, el, attrs) {
                el.bind('change', function (event) {
                    var files = event.target.files;
                    //iterate files since 'multiple' may be specified on the element
                    for (var i = 0; i < files.length; i++) {
                        //emit event upward
                        scope.$emit("fileSelected", { file: files[i] });
                    }
                });
            }
        };
    });
}());
