/// <reference path="../../scripts/angular.js" />

var app = angular.module('PermissionApp', []);

app.controller('PermissionCreate', function ($scope, $http, $window) {
    $scope.buttonDisable = true;
    $scope.validatedForCreate = false;
    var validRole = false;
    var validModule = false;

    $scope.GetAll = function (moduleId, roleId, userId) {
        if (moduleId !== undefined && moduleId !== null && roleId !== undefined && roleId !== null) {
            $http.get("/Permission/GetAllByModuleRoleOrUserId", { params: { moduleId: moduleId, roleId: roleId, userId: userId } })
                .then(function (result) {
                    $scope.List = result.data;
                    console.log(result.data);
                }), function (result) {
                    console.log(result.Status);
                    $scope.List = null;
                };
        }
    };

    $scope.GetAllRole = function () {
        $http.get("/Role/GetAll")
            .then(function (result) {
                $scope.RoleList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
    };
    //$scope.GetAllUser = function () {
    //    $http.get("/User/GetAll")
    //        .then(function (result) {
    //            $scope.UserList = result.data;
    //        }), function (result) {
    //            console.log(result.Status);
    //        };
    //};

    $scope.LoadUserByRoleId = function (roleId) {
        $http.get("/User/GetAllUserByRoleId", { params: { roleId: roleId } })
            .then(function (result) {
                $scope.UserList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
    };
    $scope.GetAllModule = function () {
        $http.get("/Module/GetAll")
            .then(function (result) {
                $scope.ModuleList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
    };

    $scope.LoadOperationByModule = function (moduleId) {
        $http.get("/Operation/GetAllByModuelId", { params: { moduleId: moduleId } })
            .then(function (result) {
                $scope.OperationByModuleList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
    };
    $scope.LoadResourceByModule = function (moduleId) {
        $http.get("/Resource/GetAllByModuelId", { params: { moduleId: moduleId } })
            .then(function (result) {
                $scope.ResourceByModuleList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
    };

    $scope.LoadResourceAndOperationByModuleId = function (moduleId, roleId, userId) {
        //$scope.LoadOperationByModule(moduleId);
        //$scope.LoadResourceByModule(moduleId);
        $scope.GetAll(moduleId, roleId, userId);
    };

    $scope.LoadResourceAndOperationByPermissionType = function (permissionType, moduleId) {
        if (permissionType === "Resource") {
            $scope.OperationByModuleList = null;
            $scope.LoadResourceByModule(moduleId);
        }
        else if (permissionType === "Operation") {
            $scope.ResourceByModuleList = null;
            $scope.LoadOperationByModule(moduleId);
        }
        else {
            $scope.LoadResourceAndOperationByModuleId(moduleId);
        }
    };
    $scope.OperationResourceArray = [];
    var permissionObject = {
        //Id: null,
        //RoleId: null,
        //UserId: null,
        //PermissionType: null,
        //OperationId: null,
        //ResourceId: null,
        //IsCreate: false,
        //IsRead: false,
        //IsEdit: false,
        //IsDelete: false,
        //IsPrint: false,
        //IsExclusive: false
    };


    $scope.CallCreate2 = function (id, value) {
        var actualId = parseInt(id.slice(0, -1));
        if (id.slice(-1) === 'o') {
            if (value === true) {
                if ($scope.OperationResourceArray.find(x => x.OperationId === actualId)) {
                    permissionObject = {};
                    permissionObject = $scope.OperationResourceArray.find(x => x.OperationId === actualId);
                    permissionObject.IsCreate = true;
                    angular.extend($scope.OperationResourceArray, permissionObject);
                }
                else {
                    permissionObject = {};
                    permissionObject.OperationId = actualId;
                    permissionObject.RoleId = $scope.permission.Role;
                    permissionObject.IsCreate = true;
                    $scope.OperationResourceArray.push(permissionObject);
                }
            } else {
                if ($scope.OperationResourceArray.find(x => x.OperationId === actualId)) {
                    permissionObject = {};
                    permissionObject = $scope.OperationResourceArray.find(x => x.OperationId === actualId);
                    permissionObject.IsCreate = false;
                    angular.extend($scope.OperationResourceArray, permissionObject);
                }
                else {
                    permissionObject = {};
                    permissionObject.OperationId = actualId;
                    permissionObject.IsCreate = false;
                    $scope.OperationResourceArray.push(permissionObject);
                }
            }
        }
        //For Resource
        if (id.slice(-1) === 'r') {
            if (value === true) {
                if ($scope.OperationResourceArray.find(x => x.ResourceId === actualId)) {
                    permissionObject = {};
                    permissionObject = $scope.OperationResourceArray.find(x => x.ResourceId === actualId);
                    permissionObject.IsCreate = true;
                    angular.extend($scope.OperationResourceArray, permissionObject);
                }
                else {
                    permissionObject = {};
                    permissionObject.RoleId = $scope.permission.Role;
                    permissionObject.ResourceId = actualId;
                    permissionObject.IsCreate = true;
                    $scope.OperationResourceArray.push(permissionObject);
                }
            } else {
                if ($scope.OperationResourceArray.find(x => x.ResourceId === actualId)) {
                    permissionObject = {};
                    permissionObject = $scope.OperationResourceArray.find(x => x.ResourceId === actualId);
                    permissionObject.IsCreate = false;
                    angular.extend($scope.OperationResourceArray, permissionObject);
                }
                else {
                    permissionObject = {};
                    permissionObject.ResourceId = actualId;
                    permissionObject.IsCreate = false;
                    $scope.OperationResourceArray.push(permissionObject);
                }
            }
        }
        console.log($scope.OperationResourceArray);
    };
    $scope.CallRead2 = function (t, value, permission) {
        //permissionObject = t;
        var actualId = parseInt(t.Id);
        if (value === true) {
            if (t.OperationId !== null) {
                if ($scope.OperationResourceArray.find(x => x.OperationId === parseInt(t.OperationId))) {
                    permissionObject = {};
                    permissionObject = $scope.OperationResourceArray.find(x => x.OperationId === parseInt(t.OperationId));
                    permissionObject.IsRead = true;
                    angular.extend($scope.OperationResourceArray, permissionObject);
                }
                else {
                    permissionObject = {};
                    permissionObject.Id = actualId;
                    permissionObject.PermissionType = permission.PermissionType;
                    permissionObject.IsRead = true;
                    $scope.OperationResourceArray.push(permissionObject);
                }
            }
            if (t.ResourceId !== null) {
                if ($scope.OperationResourceArray.find(x => x.ResourceId === parseInt(t.ResourceId))) {
                    permissionObject = {};
                    permissionObject = $scope.OperationResourceArray.find(x => x.ResourceId === parseInt(t.ResourceId));
                    permissionObject.IsRead = true;
                    angular.extend($scope.OperationResourceArray, permissionObject);
                }
                else {
                    permissionObject = {};
                    permissionObject.Id = actualId;
                    permissionObject.IsRead = true;
                    $scope.OperationResourceArray.push(permissionObject);
                }
            }
        }
        else {
            if (t.OperationId !== null) {
                if ($scope.OperationResourceArray.find(x => x.OperationId === parseInt(t.OperationId))) {
                    permissionObject = {};
                    permissionObject = $scope.OperationResourceArray.find(x => x.OperationId === parseInt(t.OperationId));
                    permissionObject.IsRead = false;
                    angular.extend($scope.OperationResourceArray, permissionObject);
                }
                else {
                    permissionObject = {};
                    permissionObject.Id = actualId;
                    permissionObject.PermissionType = permission.PermissionType;
                    permissionObject.IsRead = false;
                    $scope.OperationResourceArray.push(permissionObject);
                }
            }
            if (t.ResourceId !== null) {
                if ($scope.OperationResourceArray.find(x => x.ResourceId === parseInt(t.ResourceId))) {
                    permissionObject = {};
                    permissionObject = $scope.OperationResourceArray.find(x => x.ResourceId === parseInt(t.ResourceId));
                    permissionObject.IsRead = false;
                    angular.extend($scope.OperationResourceArray, permissionObject);
                }
                else {
                    permissionObject = {};
                    permissionObject.Id = actualId;
                    permissionObject.PermissionType = permission.PermissionType;
                    permissionObject.IsRead = false;
                    $scope.OperationResourceArray.push(permissionObject);
                }
            }
        }
    };

    $scope.CallCreate = function (t, value, permission) {
        permissionObject.Id = t.Id;
        permissionObject.IsCreate = t.IsCreate;
        permissionObject.IsRead = t.IsRead;
        permissionObject.IsEdit = t.IsEdit;
        permissionObject.IsDelete = t.IsDelete;
        permissionObject.IsPrint = t.IsPrint;
        permissionObject.IsExclusive = t.IsExclusive;
        var actualId = parseInt(t.Id);
        if (value === true) {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsCreate = true;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsCreate = true;
                $scope.OperationResourceArray.push(permissionObject);
            }            
        }
        else {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsCreate = false;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsCreate = false;
                $scope.OperationResourceArray.push(permissionObject);
            } 
        }
        permissionObject = {};
    };
    $scope.CallRead = function (t, value, permission) {
        permissionObject.Id = t.Id;
        permissionObject.IsRead = t.IsRead;
        permissionObject.IsRead = t.IsRead;
        permissionObject.IsEdit = t.IsEdit;
        permissionObject.IsDelete = t.IsDelete;
        permissionObject.IsPrint = t.IsPrint;
        permissionObject.IsExclusive = t.IsExclusive;
        var actualId = parseInt(t.Id);
        if (value === true) {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsRead = true;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsRead = true;
                $scope.OperationResourceArray.push(permissionObject);
            }
        }
        else {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsRead = false;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsRead = false;
                $scope.OperationResourceArray.push(permissionObject);
            }
        }
        permissionObject={ };
    };
    $scope.CallEdit = function (t, value, permission) {
        permissionObject.Id = t.Id;
        permissionObject.IsEdit = t.IsEdit;
        permissionObject.IsRead = t.IsRead;
        permissionObject.IsEdit = t.IsEdit;
        permissionObject.IsDelete = t.IsDelete;
        permissionObject.IsPrint = t.IsPrint;
        permissionObject.IsExclusive = t.IsExclusive;
        var actualId = parseInt(t.Id);
        if (value === true) {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsEdit = true;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsEdit = true;
                $scope.OperationResourceArray.push(permissionObject);
            }
        }
        else {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsEdit = false;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsEdit = false;
                $scope.OperationResourceArray.push(permissionObject);
            }
        }
        permissionObject = {};
    };
    $scope.CallDelete = function (t, value, permission) {
        permissionObject.Id = t.Id;
        permissionObject.IsDelete = t.IsDelete;
        permissionObject.IsRead = t.IsRead;
        permissionObject.IsEdit = t.IsEdit;
        permissionObject.IsDelete = t.IsDelete;
        permissionObject.IsPrint = t.IsPrint;
        permissionObject.IsExclusive = t.IsExclusive;
        var actualId = parseInt(t.Id);
        if (value === true) {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsDelete = true;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsDelete = true;
                $scope.OperationResourceArray.push(permissionObject);
            }
        }
        else {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsDelete = false;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsDelete = false;
                $scope.OperationResourceArray.push(permissionObject);
            }
        }
        permissionObject = {};
    };
    $scope.CallPrint = function (t, value, permission) {
        permissionObject.Id = t.Id;
        permissionObject.IsPrint = t.IsPrint;
        permissionObject.IsRead = t.IsRead;
        permissionObject.IsEdit = t.IsEdit;
        permissionObject.IsDelete = t.IsDelete;
        permissionObject.IsPrint = t.IsPrint;
        permissionObject.IsExclusive = t.IsExclusive;
        var actualId = parseInt(t.Id);
        if (value === true) {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsPrint = true;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsPrint = true;
                $scope.OperationResourceArray.push(permissionObject);
            }
        }
        else {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsPrint = false;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsPrint = false;
                $scope.OperationResourceArray.push(permissionObject);
            }
        }
        permissionObject = {};
    };
    $scope.CallExclusive = function (t, value, permission) {
        permissionObject.Id = t.Id;
        permissionObject.IsExclusive = t.IsExclusive;
        permissionObject.IsRead = t.IsRead;
        permissionObject.IsEdit = t.IsEdit;
        permissionObject.IsDelete = t.IsDelete;
        permissionObject.IsPrint = t.IsPrint;
        permissionObject.IsExclusive = t.IsExclusive;
        var actualId = parseInt(t.Id);
        if (value === true) {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsExclusive = true;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsExclusive = true;
                $scope.OperationResourceArray.push(permissionObject);
            }
        }
        else {
            if ($scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id))) {
                permissionObject = $scope.OperationResourceArray.find(x => x.Id === parseInt(t.Id));
                permissionObject.IsExclusive = false;
                angular.extend($scope.OperationResourceArray, permissionObject);
            }
            else {
                permissionObject.IsExclusive = false;
                $scope.OperationResourceArray.push(permissionObject);
            }
        }
        permissionObject = {};
    };


    $scope.CheckValidation = function (permission) {
        if (permission.Role !== undefined && permission.Role !== null) {
            if (permission.Role === "") {
                validRole = false;
            } else {
                validRole = true;
            }
        }
        if (permission.Module !== undefined && permission.Module !== null) {
            if (permission.Module === "") {
                validModule = false;
            } else {
                validModule = true;
            }
        }
        if (validRole === true && validModule === true) {
            $scope.buttonDisable = false;
            $scope.validatedForCreate = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForCreate = false;
        }
    };
    $scope.SaveAndUpdate = function (permission) {
        $scope.CheckValidation(permission);
        if ($scope.validatedForCreate === true) {
            var data = $scope.OperationResourceArray;
            $http({
                method: "post",
                url: "/Permission/CreateAndUpdate",
                data: data
                //or data: JSON.stringify(data) ??
            }).then(function (result) {
                $scope.permission = {};
                alert(result.data);
                $window.location.href = "/Permission/Create";
            }), function (result) {
                alert(result.data);
            };
        }
    };

    $scope.Reset = function (permission) {
        $scope.OperationResourceArray = [];
        permissionObject = {};
        $window.location.href = "/Permission/Create";
    };

    $scope.Init = function () {
        $scope.GetAllRole();
        $scope.GetAllModule();
    };
});