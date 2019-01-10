/// <reference path="../../scripts/angular.js" />

var app = angular.module('LoginApp',[]);
app.controller('Login', function ($scope, $http, $window) {
    $scope.errorUserNamePassword = false;
    $scope.Go = function (user) {
        if (user.UserName !== null && user.Password !== null) {
            $http.post("/Home/Login", user)
                .then(function (result) {
                    $scope.user = {};
                    if (result.data === "invalid") {
                        $scope.errorUserNamePassword = true;
                        //$window.location.href = "/Home/Login";
                    }
                    else {
                        if (result.data === "ok") {
                            $scope.errorUserNamePassword = false;
                            $window.location.href = "/Role/Index";
                        }
                    }
                }, function (result) {
                    
                    alert(result.data);
                });
        }
        else {
            alert("username or password can not be empty");
        }
    };
});