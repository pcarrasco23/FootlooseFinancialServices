// module.js
var module = angular.module('homeIndex', ['ngRoute', 'xeditable']);

module.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when("/", {
            controller: "homeIndexController",
            templateUrl: "/templates/homeIndexView.html"
        })
        .when("/contactinfo", {
            controller: "contactInfoController",
            templateUrl: "/templates/contactInfoView.html"
        })
        .when("/changepassword", {
            controller: "changePasswordController",
            templateUrl: "/templates/changePassword.html"
        })
        .otherwise({ redirectTo: "/" });
}]);

module.run(function (editableOptions) {
    editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'
});
