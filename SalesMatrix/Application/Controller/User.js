/// <reference path="../../scripts/angular.js" />

var app = angular.module('UserApp', ['ngFileUpload']);
app.controller('UserCreate', function ($scope, $http, $window, Upload) {
    $scope.Init = function () {
        $scope.GetActiveRole();
    };
    $scope.GetActiveRole = function () {
        $http.get("/Role/GetAllActiveRole")
            .then(function (response) {
                $scope.Role = response.data;
            }), function (response) {
                console.log(response.status);
            };
    };
    $scope.user_Status = "true";
    $scope.buttonDisable = true;
    $scope.UserNameExist = false;

    var validComparePassword = false;
    var roleSelected = false;

    var validUserName = false;
    var validFullName = false;
    var validEmail = false;
    var validPassword = false;
    var validRoleId = false;
    $scope.CheckUserName = function (UserName) {
        $http.get("/User/UserNameExists", { params: { userName: UserName } })
            .then(function (response) {
                if (response.data === true) {
                    $scope.errorMsgUserNameExist = true;
                    validUserName = false;
                }
                else {
                    $scope.errorMsgUserNameExist = false;
                    validUserName = true;
                }
            }), function (response) {
                alert(response.status);
            };
    };
    $scope.CheckValidation = function (user) {
        if (user.UserName !== undefined && user.UserName !== null) {
            if (user.UserName === "") {
                validUserName = false;
            } else {
                validUserName = true;
            }
        }
        if (user.FullName !== undefined && user.FullName !== null) {
            if (user.FullName === "") {
                validFullName = false;
            } else {
                validFullName = true;
            }
        }
        if (user.Email !== undefined && user.Email !== null) {
            if (user.Email === "") {
                validEmail = false;
            } else {
                validEmail = true;
            }
        }
        if (user.Password !== undefined && user.Password !== null) {
            if (user.Password === "") {
                validPassword = false;
            } else {
                validPassword = true;
            }
        }
        if (user.RoleId !== undefined && user.RoleId !== null) {
            if (user.RoleId === "") {
                validRoleId = false;
                $scope.errorMsgInvalidRole = true;
            } else {
                validRoleId = true;
                $scope.errorMsgInvalidRole = false;
            }
        }

        if (validUserName === true && validFullName === true && validEmail === true && validPassword === true && validComparePassword === true && validRoleId === true) {
            $scope.buttonDisable = false;
            $scope.validatedForCreate = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForCreate = false;
        }
    };

    $scope.ComparePassword = function (password, confirmPassword) {
        if (password === confirmPassword) {
            $scope.UnMatchedPassword = false;
            validComparePassword = true;
        } else {
            $scope.UnMatchedPassword = true;
            validComparePassword = false;
        }
    };
    $scope.RoleDropdown = function (value) {
        if (value === undefined || value === null || value === "") {
            roleSelected = false;
            $scope.InvalidRole = true;
        } else {
            roleSelected = true;
            $scope.InvalidRole = false;
        }
        $scope.CheckValidation();
    };

    $scope.Check = function () {
        if (validUserName === true && validFullName === true && validEmail === true && validPassword === true && validComparePassword === true && validRoleId === true) {
            $scope.buttonDisable = false;
            $scope.validatedForCreate = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForCreate = false;
        }
    };
    $scope.UploadFiles = function (files) {
        $scope.SelectedFiles = files;
    };

    $scope.Create = function (user) {
        $scope.CheckValidation(user);
        if ($scope.validatedForCreate === true) {
            user.Status = $scope.user_Status;
            if ($scope.SelectedFiles && $scope.SelectedFiles.length) {
                Upload.upload({
                    url: '/User/Upload/',
                    data: {
                        files: $scope.SelectedFiles
                    }
                }).then(function (r) {
                    if (r.data === "Success") {
                        $http.post("/User/Create", user)
                            .then(function (result) {
                                alert(result.data);
                                $scope.user = {};
                                $window.location.href = "/User/Index";
                            }, function (result) {
                                alert(result.status);
                            });
                    } else {
                        alert("Image Upload Problem");
                    }

                }), function () {
                    alert(result.status);
                };
            }

        }
    };
});

app.controller('UserEdit', function ($scope, $http, $window, Upload) {
    $scope.Init = function () {
        $scope.GetActiveRole();
    };
    $scope.GetActiveRole = function () {
        $http.get("/Role/GetAllActiveRole")
            .then(function (response) {
                $scope.Role = response.data;
            }), function (response) {
                console.log(response.status);
            };
    };
    $scope.buttonDisable = false;
    $scope.UserNameExist = false;
    var validUserName = true;
    var validFullName = true;
    var validEmail = true;
    var validroleId = true;


    var validName = true;
    var roleSelected = true;
    $scope.CheckValidation = function (user) {
        if (user !== undefined && user !== null) {
            if (user.UserName !== undefined && user.UserName !== null && user.UserName !== "") {
                validUserName = true;
            } else {
                validUserName = false;
            }
            if (user.FullName !== undefined && user.FullName !== null && user.FullName !== "") {
                validFullName = true;
            } else {
                validFullName = false;
            }
            if (user.Email !== undefined && user.Email !== null && user.Email !== "") {
                validEmail = true;
            } else {
                validEmail = false;
            }
            if ($scope.roleId === undefined || $scope.roleId === null || $scope.roleId === "") {
                validroleId = false;
            } else {
                validroleId = true;
            }
        }
        if (validUserName === true && validFullName === true && validEmail === true && validroleId === true && validName === true) {
            $scope.buttonDisable = false;
            $scope.validatedForEdit = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForEdit = false;
        }
    };

    $scope.CheckNameWithId = function (UserName, UserId) {
        $http.get("/User/UserExistsForEdit", { params: { name: UserName, id: UserId } })
            .then(function (response) {
                if (response.data === true) {
                    $scope.UserNameExist = true;
                    validName = false;
                }
                else {
                    $scope.UserNameExist = false;
                    validName = true;
                }
            }), function (response) {
                alert(response.Status);
            };
        $scope.CheckValidation();
    };

    $scope.RoleDropdown = function (value) {
        if (value === undefined || value === null || value === "") {
            roleSelected = false;
            $scope.InvalidRole = true;
        } else {
            roleSelected = true;
            $scope.InvalidRole = false;
        }
        $scope.CheckValidation();
    };
    $scope.UploadFiles = function (files) {
        $scope.SelectedFiles = files;
    };

    $scope.GetById = function (id) {
        $http.get("/User/GetById", { params: { id: id } })
            .then(function (result) {
                $scope.user = result.data;
                $scope.roleId = result.data.RoleId;
                if (result.data.Status === true) {
                    $scope.user_Status = 'true';
                } else {
                    $scope.user_Status = 'false';
                }
                if (result.data.RoleId === null) {
                    roleSelected = false;
                }
                else {
                    roleSelected = true;
                }
                $scope.CheckNameWithId(result.data.UserName, id);

            }), function (result) {
                console.log(result.Status);
            };
        $scope.CheckValidation();
    };
    $scope.Edit = function (user) {
        $scope.CheckValidation(user);
        user.Status = $scope.user_Status;
        user.RoleId = $scope.roleId;
        if ($scope.validatedForEdit === true) {
            //if ($scope.SelectedFiles && $scope.SelectedFiles.length) {
            //    Upload.upload({
            //        url: '/User/Upload/',
            //        data: {
            //            files: $scope.SelectedFiles   
            //        }
            //    });
            //}
            $http.post("/User/Edit", user)
                .then(function (result) {
                    alert(result.data);
                    $scope.user = {};
                    $window.location.href = "/User/Index";
                }, function (result) {
                    alert(result.status);
                });
        }
    };
});


app.controller('UserList', function ($scope, $http, $filter) {

    $scope.Init = function () {
        $scope.GetAllActiveUser();
        $scope.GetAllRole();
    };
    $scope.Active = true;

    $scope.GetAllActiveUser = function () {
        $http.get("/User/GetAllActiveUser")
            .then(function (result) {
                $scope.UserList = result.data;
                $scope.pagination($scope.UserList);
            }), function (response) {
                console.log(response.status);
            };
    };

    $scope.GetAllInactiveUser = function () {
        $http.get("/User/GetAllInactiveUser")
            .then(function (result) {
                $scope.UserList = result.data;
                $scope.pagination($scope.UserList);
            }), function (response) {
                console.log(response.status);
            };
    };

    $scope.GetAllRole = function () {
        $http.get("/Role/GetAll")
            .then(function (response) {
                $scope.RoleList = response.data;
            }), function (response) {
                console.log(response.status);
            };
    };

    $scope.ClickActiveInactive = function (e) {
        if ($scope.Active === true) {
            $scope.GetAllInactiveUser();
        } else {
            $scope.GetAllActiveUser();
        }
    };

    $scope.ResetPassword = function (e) {
        $http.get("/User/ResetPassword", { params: { id: e } })
            .then(function (result) {
                alert(result.data);
            }), function (result) {
                console.log(result.Status);
            };


    }

    $scope.pagination = function (list) {
        $scope.sort = {
            sortingOrder: 'id',
            reverse: false
        };
        $scope.filteredItems = [];
        $scope.groupedItems = [];
        $scope.itemsPerPage = 10;
        $scope.pagedItems = [];
        $scope.currentPage = 0;
        $scope.items = list;
        if (($scope.items.length % $scope.itemsPerPage) === 0) {
            $scope.gap = parseInt(($scope.items.length / $scope.itemsPerPage));
        } else {
            $scope.gap = parseInt(($scope.items.length / $scope.itemsPerPage) + 1);
        }

        var searchMatch = function (haystack, needle) {
            if (!needle) {
                return true;
            }
            return haystack.toLowerCase().indexOf(needle.toLowerCase()) !== -1;
        };

        // init the filtered items
        $scope.search = function () {
            $scope.filteredItems = $filter('filter')($scope.items, function (item) {
                for (var attr in item) {
                    if (searchMatch(item[attr], $scope.query))
                        return true;
                }
                return false;
            });
            // take care of the sorting order
            if ($scope.sort.sortingOrder !== '') {
                $scope.filteredItems = $filter('orderBy')($scope.filteredItems, $scope.sort.sortingOrder, $scope.sort.reverse);
            }
            $scope.currentPage = 0;
            // now group by pages
            $scope.groupToPages();
        };


        // calculate page in place
        $scope.groupToPages = function () {
            $scope.pagedItems = [];

            for (var i = 0; i < $scope.filteredItems.length; i++) {
                if (i % $scope.itemsPerPage === 0) {
                    $scope.pagedItems[Math.floor(i / $scope.itemsPerPage)] = [$scope.filteredItems[i]];
                } else {
                    $scope.pagedItems[Math.floor(i / $scope.itemsPerPage)].push($scope.filteredItems[i]);
                }
            }
        };

        $scope.range = function (size, start, end) {
            var ret = [];
            if (size < end) {
                end = size;
                start = size - $scope.gap;
            }
            for (var i = start; i < end; i++) {
                ret.push(i);
            }
            return ret;
        };

        $scope.prevPage = function () {
            if ($scope.currentPage > 0) {
                $scope.currentPage--;
            }
        };

        $scope.nextPage = function () {
            if ($scope.currentPage < $scope.pagedItems.length - 1) {
                $scope.currentPage++;
            }
        };

        $scope.setPage = function () {
            $scope.currentPage = this.n;
        };

        // functions have been describe process the data for display
        $scope.search();
    };
});

app.controller('UserDetails', function ($scope, $http) {


});