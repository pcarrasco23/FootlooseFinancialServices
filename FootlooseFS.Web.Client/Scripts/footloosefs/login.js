// login.js
var module = angular.module('login', ['ngRoute']);

module.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when("/", {
            controller: "loginController",
            templateUrl: "/templates/login.html"
        })
        .otherwise({ redirectTo: "/" });
}]);

module.controller('loginController', ['$scope', '$http', 'dataService', function ($scope, $http, dataService) {
    $scope.isBusy = false;
    $scope.isError = false;
    $scope.ErrorMessage = "";

    $scope.username = "";
    $scope.password = "";

    $scope.submit = function () {
        $scope.isBusy = true;

        dataService.Login($scope.username, $scope.password)
            .then(function (data) {
                // success

                dataService.GetDashboard()
                    .then(function (data) {
                        // success
                        sessionStorage.setItem("firstName", data.FirstName);
                        sessionStorage.setItem("lastName", data.LastName);

                        location.href = "/";
                    },
                    function (errorMessage) {
                        // error
                        alert(errorMessage);
                    })
                    .then(function () {
                        $scope.isBusy = false;
                    });
            },
            function (errorMessage) {
                // error
                $scope.isBusy = false;
                $scope.isError = true;
                $scope.ErrorMessage = "The username and password combination is not correct";
            })
            .then(function () {
                $scope.isBusy = false;
            });
    }    
}]);