module.controller('registerController', ['$scope', 'dataService', function ($scope, dataService) {
    $scope.isBusy = false;
    $scope.isEnrolled = false;
    $scope.isError = false;
    $scope.ErrorMessage = "";

    $scope.data = {
        LastName: '',
        AccountNumber: '',
        Username: '',
        Password: '',
        ConfirmPassword: ''
    }

    $scope.submit = function () {
        $scope.isBusy = true;

        dataService.Register($scope.data)
            .then(function (data) {
                $scope.isBusy = false;
                $scope.isEnrolled = true;
                $scope.isError = false;
            },
            function (errorMessage) {
                $scope.isBusy = false;
                $scope.isError = true;
                $scope.ErrorMessage = errorMessage;
            });
    }

    $scope.back = function () {
        $scope.data.LastName = '';
        $scope.data.AccountNumber = '';
        $scope.data.Username = '';
        $scope.data.Password = '';
        $scope.data.ConfirmPassword = '';
    }
}]);