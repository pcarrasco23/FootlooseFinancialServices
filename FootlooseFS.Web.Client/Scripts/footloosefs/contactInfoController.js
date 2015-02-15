module.controller('contactInfoController', ['$scope', '$http', '$route', '$location', 'dataService', function ($scope, $http, $route, $location, dataService) {
    $scope.isBusy = true;
    $scope.isConfirm = false;
    $scope.isUpdated = false;
    $scope.isError = false;
    $scope.ErrorMessage = '';

    $scope.data = {
        EmailAddress: "",
        Addresses: [],
        PhoneNumbers: []
    };

    dataService.GetContactInfo()
        .then(function (data) {
            // success
            $scope.data.EmailAddress = data.EmailAddress;

            angular.copy(data.Addresses, $scope.data.Addresses);
            angular.copy(data.PhoneNumbers, $scope.data.PhoneNumbers);

            $scope.isBusy = false;
        },
        function () {
            $scope.isBusy = false;
            $scope.IsError = true;
            $cope.ErrorMessage = "There was an error";
        })

    $scope.states = [
        { value: '', text: '' },
        { value: 'AL', text: 'AL' },
        { value: 'AK', text: 'AK' },
        { value: 'AS', text: 'AS' },
        { value: 'AZ', text: 'AZ' },
        { value: 'AR', text: 'AR' },
        { value: 'CA', text: 'CA' },
        { value: 'CO', text: 'CO' },
        { value: 'CT', text: 'CT' },
        { value: 'DE', text: 'DE' },
        { value: 'DC', text: 'DC' },
        { value: 'FM', text: 'FM' },
        { value: 'FL', text: 'FL' },
        { value: 'GA', text: 'GA' },
        { value: 'GU', text: 'GU' },
        { value: 'HI', text: 'HI' },
        { value: 'ID', text: 'ID' },
        { value: 'IL', text: 'IL' },
        { value: 'IN', text: 'IN' },
        { value: 'IA', text: 'IA' },
        { value: 'KS', text: 'KS' },
        { value: 'KY', text: 'KY' },
        { value: 'LA', text: 'LA' },
        { value: 'ME', text: 'ME' },
        { value: 'MH', text: 'MH' },
        { value: 'MD', text: 'MD' },
        { value: 'MA', text: 'MA' },
        { value: 'MI', text: 'MI' },
        { value: 'MN', text: 'MN' },
        { value: 'MS', text: 'MS' },
        { value: 'MO', text: 'MO' },
        { value: 'MT', text: 'MT' },
        { value: 'NE', text: 'NE' },
        { value: 'NV', text: 'NV' },
        { value: 'NH', text: 'NH' },
        { value: 'NJ', text: 'NJ' },
        { value: 'NM', text: 'NM' },
        { value: 'NY', text: 'NY' },
        { value: 'NC', text: 'NC' },
        { value: 'ND', text: 'ND' },
        { value: 'MP', text: 'MP' },
        { value: 'OH', text: 'OH' },
        { value: 'OK', text: 'OK' },
        { value: 'OR', text: 'OR' },
        { value: 'PA', text: 'PA' },
        { value: 'PR', text: 'PR' },
        { value: 'RI', text: 'RI' },
        { value: 'SC', text: 'SC' },
        { value: 'SD', text: 'SD' },
        { value: 'TN', text: 'TN' },
        { value: 'TX', text: 'TX' },
        { value: 'UT', text: 'UT' },
        { value: 'VT', text: 'VT' },
        { value: 'VI', text: 'VI' },
        { value: 'VA', text: 'VA' },
        { value: 'WA', text: 'WA' },
        { value: 'WV', text: 'WV' },
        { value: 'WI', text: 'WI' },
        { value: 'WY', text: 'WY' }
    ];

    $scope.cancel = function () {
        $route.reload();
    }

    $scope.confirm = function () {
        $scope.isConfirm = true;
    }

    $scope.back = function () {
        $scope.isConfirm = false;
        $scope.isUpdated = false;
    }

    $scope.submit = function () {
        $scope.isBusy = true;

        dataService.PutContactInfo($scope.data)
            .then(function (result) {
                $scope.isUpdated = true;
                $scope.isBusy = false;
                $scope.isConfirm = false;
            },
            function (errorMessage) {
                $scope.isBusy = false;
                $scope.isError = true;
                $scope.ErrorMessage = errorMessage;
            });
    }

    $scope.clearAddress = function (address) {
        for (i = 0; i < $scope.data.Addresses.length; i++) {
            if ($scope.data.Addresses[i].Type == address.Type) {
                $scope.data.Addresses[i] = {
                    AddressID: address.AddressID,
                    Type: address.Type,
                    StreetAddress: '',
                    City: '',
                    State: '',
                    Zip: ''
                };
            }
        }
    }

    $scope.clearPhone = function (phone) {
        for (i = 0; i < $scope.data.PhoneNumbers.length; i++) {
            if ($scope.data.PhoneNumbers[i].PhoneType == phone.PhoneType) {
                $scope.data.PhoneNumbers[i] = {
                    PhoneType: phone.PhoneType,
                    Number: ''
                };
            }
        }
    }    
}]);