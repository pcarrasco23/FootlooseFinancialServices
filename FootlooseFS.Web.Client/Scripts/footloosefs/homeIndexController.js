module.controller('homeIndexController', ['$scope', '$http', 'dataService', function ($scope, $http, dataService) {
    $scope.isBusy = true;

    $scope.Accounts = [];
    $scope.Transactions = [];
    $scope.Total = 0.0;

    dataService.GetAccounts()
        .then(function (data) {
            // success
            $scope.Total = data.Total;

            angular.copy(data.Accounts, $scope.Accounts);
            angular.copy(data.Transactions, $scope.Transactions);
        },
        function (errorMessage) {
            // error
            alert(errorMessage);
        })
        .then(function () {
            $scope.isBusy = false;
        });
}]);