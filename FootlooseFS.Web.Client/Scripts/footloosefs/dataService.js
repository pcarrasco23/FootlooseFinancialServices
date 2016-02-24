module.factory('dataService', function ($http, $q) {
    var service = {};
    var url = config.serviceUrl;

    service.Login = function (username, password) {
        var loginRequest = "grant_type=password&username=" + username + "&password=" + password;
        var deferred = $q.defer();

        $http.post(url + '/Token', loginRequest, { headers: { "Content-Type": "application/x-www-form-urlencoded" } }).
          success(function (data, status, headers, config) {
              // Cache the access token in session storage.
              sessionStorage.setItem("accessToken", data.access_token);

              deferred.resolve(data);
          }).
          error(function (data, status, headers, config) {
              // Remove the cached access token
              sessionStorage.removeItem("accessToken");

              deferred.reject('There was an error');
          });

        return deferred.promise;
    }

    service.GetDashboard = function () {
        var deferred = $q.defer();

        $http.get(url + '/api/dashboard/', { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem("accessToken") } }).
          success(function (data, status, headers, config) {
              deferred.resolve(data);
          }).
          error(function (data, status, headers, config) {
              deferred.reject('There was an error');
          });

        return deferred.promise;
    }

    service.GetAccounts = function () {
        var deferred = $q.defer();

        $http.get(url + '/api/accounts/', { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem("accessToken") } }).
          success(function (data, status, headers, config) {
              deferred.resolve(data);
          }).
          error(function (data, status, headers, config) {

              if (status == 401) {
                  location.href = "/login.html";
                  deferred.reject('You are not authorized to perform this action.');
              }                  
              else {
                  deferred.reject('There was an error');
              }
          });

        return deferred.promise;
    }

    service.GetContactInfo = function () {
        var deferred = $q.defer();

        $http.get(url + '/api/contactinfo/', { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem("accessToken") } }).
          success(function (data, status, headers, config) {
              deferred.resolve(data);
          }).
          error(function (data, status, headers, config) {
              deferred.reject('There was an error');
          });

        return deferred.promise;
    }

    service.PutContactInfo = function (contactInfo) {
        var deferred = $q.defer();

        $http.put(url + '/api/contactinfo/', contactInfo, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem("accessToken") } }).
        success(function (data, status, headers, config) {
            deferred.resolve(data);
        }).
        error(function (data, status, headers, config) {
            deferred.reject('There was an error');
        });

        return deferred.promise;
    }

    service.ChangePassword = function (changePasswordViewModel) {
        var deferred = $q.defer();

        $http.put(url + '/api/changepassword/', changePasswordViewModel, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem("accessToken") } }).
        success(function (data, status, headers, config) {
            deferred.resolve(data);
        }).
        error(function (data, status, headers, config) {
            deferred.reject('There was an error');
        });

        return deferred.promise;
    }

    service.Register = function (registerViewModel) {
        var deferred = $q.defer();

        $http.put(url + '/api/register/', registerViewModel, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem("accessToken") } }).
        success(function (data, status, headers, config) {
            deferred.resolve(data);
        }).
        error(function (data, status, headers, config) {
            deferred.reject(data.ExceptionMessage);
        });

        return deferred.promise;
    }

    return service;
});