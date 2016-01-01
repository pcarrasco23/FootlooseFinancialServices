module.controller('changePasswordController', ['$scope', '$http', '$location', 'dataService', function ($scope, $http, $location, dataService) {
    $scope.isBusy = false;
    $scope.isUpdated = false;
    $scope.isError = false;
    $scope.ErrorMessage = "";

    $scope.data = {
        OldPassword: '',
        NewPassword: '',
        ConfirmNewPassword: ''
    }

    $scope.submit = function () {
        $scope.isBusy = true;

        dataService.ChangePassword($scope.data)
            .then(function (data) {
                $scope.isBusy = false;
                $scope.isUpdated = true;
            },
            function (errorMessage) {
                $scope.isBusy = false;
                $scope.isError = true;
                $scope.ErrorMessage = errorMessage;
            });
    }

    $scope.back = function () {
        $scope.isUpdated = false;

        $scope.data.OldPassword = '';
        $scope.data.NewPassword = '';
        $scope.data.ConfirmNewPassword = '';
    }
}]);